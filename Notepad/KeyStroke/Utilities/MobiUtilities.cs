 

namespace Utilities
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using Common;
	
	public static class MobiUtilities
	{
		   static Dictionary<string, string> GetMetadata(string fullPath)
        {
            var dictionary = new Dictionary<string, string>();

            var fileStream = File.Open(fullPath, FileMode.Open);


            var metadata = new MobiMetadata(fileStream);

            fileStream.Close();
            string author = null;

            if (metadata.MobiHeader.EXTHHeader != null)
            {
                author = metadata.MobiHeader.EXTHHeader.FieldList["Author"].ToString();
                if (author != null)
                {
                    author = author.Replace("; ", " & ");
                }
            }

            string title = null;
            if (metadata.MobiHeader.EXTHHeader != null)
            {
                title = metadata.MobiHeader.EXTHHeader.FieldList["UpdatedTitle"].ToString();
                if (title != null)
                {
                    title = title.Trim('"')
                        .Split('"')
                        .First()
                        .Trim();
                }
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                title = metadata.MobiHeader.FullName;
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                title = metadata.PDBHeader.Name.Replace('_', ' ');
            }
            if (title != null)
            {
                title.Split(";:,\n".ToCharArray()).First().Trim();
            }
            if (author.IsWhiteSpace())
            {
                author = "Anonymous";
            }
            else
            {
                author = author.GetValidFileName();
            }

            if (title.IsWhiteSpace())
            {
                title = "Unknown";
            }
            else
            {
                title = title.GetValidFileName();
            }
            dictionary.Add(author, title);
            return dictionary;
        }

        static string ProcessAuthor(string author)
        {
            if (author.IsWhiteSpace())
            {
                return "Anonymous";
            }
            else if (author.Contains(","))
            {
                var ds = author.Split(',');
                author = ds[1].Trim() + " " + ds.First().Trim();
            }
            return author.Split('；').First().Trim().GetValidFileName();
        }

        static string ProcessTitle(string title)
        {
            if (title.IsWhiteSpace())
            {
                return "Unknown";
            }
            else
            {
                title = title.Split(new char[] {';', '-', '/', ':'}).First().Trim();
            }
            return title.GetUniqueFileName();
        }

        static void ProcessEbook(string fileName, string targetDirectory, string duplicateDirectory)
        {
           
            var dictionary = GetMetadata(fileName);

            string author = dictionary.Keys.First().Replace("\n","").Split(new char[]{'\n','；',',',';'}).First();
            string title = dictionary.Values.First().Split(new char[]{'\n','；',',',';'}).First();

            var targetFileName = Path.Combine(targetDirectory, String.Format("{0} - {1}{2}",title,author,Path.GetExtension(fileName)));
            if (File.Exists(targetFileName))
            {
            	targetFileName = Path.Combine(duplicateDirectory,String.Format("{0} - {1}{2}",title,author,Path.GetExtension(fileName)));
                targetFileName = targetFileName.GetUniqueFileName();

                File.Move(fileName, targetFileName);
            }
            else
            {
                File.Move(fileName, targetFileName);
            }
            
        }
		public static void PrettyName(this string dir)
		{
			
			var sourceDirectory =dir;
			var regex = new Regex("\\.(?:mobi|azw3|azw)$");

			var files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories)
                    .Where(i => regex.IsMatch(i) && !i.GetDirectoryFileName().StartsWith("."));
			var target = Path.Combine(sourceDirectory, ".MOBI");
			target.CreateDirectoryIfNotExists();
			var duplicate = Path.Combine(sourceDirectory, ".DUPLICATE");
			duplicate.CreateDirectoryIfNotExists();

			if (!files.Any()) {
				
			} else {
				Task.Run(() => {
					for (int i = 0; i < files.Count(); i++) {
						try {
							ProcessEbook(files.ElementAt(i), target, duplicate);
						} catch (Exception e) {
							Console.WriteLine(e);
						}
					}
				});
			}

		}
	}

	
	
	public class BaseHeader
	{
		protected SortedDictionary<string, object> fieldList = new SortedDictionary<string, object>();
		protected SortedDictionary<string, object> fieldListNoBlankRows = new SortedDictionary<string, object>();

		protected SortedDictionary<string, object> emptyFieldList =
			new SortedDictionary<string, object>();
		//Used to get properties for a blank record

		private List<string> fieldListExclude =
			new List<string>() {
				"FieldList",
				"FieldListNoBlankRows",
				"EmptyFieldList",
				"EXTHHeader"
			};

		public SortedDictionary<string, object> FieldList {
			get { return this.fieldList; }
		}

		public SortedDictionary<string, object> FieldListNoBlankRows {
			get { return this.fieldListNoBlankRows; }
		}

		public SortedDictionary<string, object> EmptyFieldList {
			get { return this.emptyFieldList; }
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool showBlankRows)
		{
			StringBuilder sb = new StringBuilder();

			if (showBlankRows) {
				foreach (KeyValuePair<string, object> kp in this.fieldList) {
					sb.AppendLine(String.Format("{0}: {1}", kp.Key, kp.Value));
				}
			} else {
				foreach (KeyValuePair<string, object> kp in this.fieldListNoBlankRows) {
					sb.AppendLine(String.Format("{0}: {1}", kp.Key, kp.Value));
				}
			}


			return sb.ToString();
		}

		protected void PopulateFieldList()
		{
			PopulateFieldList(false);
		}

		protected void PopulateFieldList(bool blankOnly)
		{
			fieldList.Clear();
			emptyFieldList.Clear();
			foreach (System.Reflection.PropertyInfo propinfo in this.GetType().GetProperties()) {
				if (fieldListExclude.Contains(propinfo.Name) == false) {
					if (!blankOnly) {
						fieldList.Add(propinfo.Name, propinfo.GetValue(this, null));
						if (propinfo.GetValue(this, null).ToString() != String.Empty) {
							fieldListNoBlankRows.Add(propinfo.Name, propinfo.GetValue(this, null));
						}
					}
					emptyFieldList.Add(propinfo.Name, null);
				}
			}
		}
	}

	public static class Converter
	{
		public static short ToInt16(byte[] bytes)
		{
			return BitConverter.ToInt16(CheckBytes(bytes), 0);
		}

		public static int ToInt32(byte[] bytes)
		{
			return BitConverter.ToInt32(CheckBytes(bytes), 0);
		}

		public static long ToInt64(byte[] bytes)
		{
			return BitConverter.ToInt64(CheckBytes(bytes), 0);
		}

		public static ushort ToUInt16(byte[] bytes)
		{
			return BitConverter.ToUInt16(CheckBytes(bytes), 0);
		}

		public static uint ToUInt32(byte[] bytes)
		{
			return BitConverter.ToUInt32(CheckBytes(bytes), 0);
		}

		public static ulong ToUInt64(byte[] bytes)
		{
			return BitConverter.ToUInt64(CheckBytes(bytes), 0);
		}

		//Checks to see if system architecture is little-endian (e.g. little end first) and if so reverse the byte array
		private static byte[] CheckBytes(byte[] bytesToCheck)
		{
			//Make copy so we're not permanently reversing the order of the bytes in the actual field
			byte[] buffer = (byte[])bytesToCheck.Clone();

			if (BitConverter.IsLittleEndian)
				Array.Reverse(buffer);

			return buffer;
		}
	}

	public class EXTHHead : BaseHeader
	{
		private byte[] identifier = new byte[4];
		private byte[] headerLength = new byte[4];
		private byte[] recordCount = new byte[4];

		private List<EXTHRecord> recordList = new List<EXTHRecord>();

		public EXTHHead()
		{
			PopulateFieldList(true);
		}

		public EXTHHead(FileStream fs)
		{
			fs.Read(this.identifier, 0, this.identifier.Length);

			if (this.IdentifierAsString != "EXTH") {
				throw new IOException("Expected to find EXTH header identifier EXTH but got something else instead");
			}

			fs.Read(this.headerLength, 0, this.headerLength.Length);
			fs.Read(this.recordCount, 0, this.recordCount.Length);

			for (int i = 0; i < this.RecordCount; i++) {
				this.recordList.Add(new EXTHRecord(fs));
			}

			PopulateFieldList();
		}

		protected int DataSize {
			get {
				int size = 0;
				foreach (EXTHRecord rec in this.recordList) {
					size += rec.Size;
				}

				return size;
			}
		}

		public int Size {
			get {
				int dataSize = this.DataSize;
				return 12 + dataSize + GetPaddingSize(dataSize);
			}
		}

		protected int GetPaddingSize(int dataSize)
		{
			int paddingSize = dataSize % 4;
			if (paddingSize != 0)
				paddingSize = 4 - paddingSize;

			return paddingSize;
		}


		//Properties
		public string IdentifierAsString {
			get { return Encoding.UTF8.GetString(this.identifier).Replace("\0", String.Empty); }
		}

		public uint HeaderLength {
			get { return Converter.ToUInt32(this.headerLength); }
		}

		public uint RecordCount {
			get { return Converter.ToUInt32(this.recordCount); }
		}

		public string Author {
			get { return GetRecordByType(100); }
		}

		public string Publisher {
			get { return GetRecordByType(101); }
		}

		public string Imprint {
			get { return GetRecordByType(102); }
		}

		public string Description {
			get { return GetRecordByType(103); }
		}

		public string IBSN {
			get { return GetRecordByType(104); }
		}

		public string Subject {
			get { return GetRecordByType(105); }
		}

		public string PublishedDate {
			get { return GetRecordByType(106); }
		}

		public string Review {
			get { return GetRecordByType(107); }
		}

		public string Contributor {
			get { return GetRecordByType(108); }
		}

		public string Rights {
			get { return GetRecordByType(109); }
		}

		public string SubjectCode {
			get { return GetRecordByType(110); }
		}

		public string Type {
			get { return GetRecordByType(111); }
		}

		public string Source {
			get { return GetRecordByType(112); }
		}

		public string ASIN {
			get { return GetRecordByType(113); }
		}

		public string VersionNumber {
			get { return GetRecordByType(114); }
		}

		public string RetailPrice {
			get { return GetRecordByType(118); }
		}

		public string RetailPriceCurrency {
			get { return GetRecordByType(119); }
		}

		public string DictionaryShortName {
			get { return GetRecordByType(200); }
		}

		public string CDEType {
			get { return GetRecordByType(501); }
		}

		public string UpdatedTitle {
			get { return GetRecordByType(503); }
		}

		public string ASIN2 {
			get { return GetRecordByType(504); }
		}


		private string GetRecordByType(int recType)
		{
			string record = String.Empty;
			foreach (EXTHRecord rec in this.recordList) {
				if (rec.RecordType == recType) {
					record = System.Text.Encoding.UTF8.GetString(rec.RecordData);
				}
			}
			return record;
		}
	}

	public class EXTHRecord
	{
		private byte[] recordType = new byte[4];
		private byte[] recordLength = new byte[4];
		private byte[] recordData = null;

		public EXTHRecord(FileStream fs)
		{
			fs.Read(this.recordType, 0, this.recordType.Length);
			fs.Read(this.recordLength, 0, this.recordLength.Length);

			if (this.RecordLength < 8)
				throw new IOException("Invalid EXTH record length");
			this.recordData = new byte[this.RecordLength - 8];
			fs.Read(this.recordData, 0, this.recordData.Length);
		}

		//Properties
		public int DataLength {
			get { return this.recordData.Length; }
		}

		public int Size {
			get { return DataLength + 8; }
		}

		public uint RecordLength {
			get { return Converter.ToUInt32(this.recordLength); }
		}

		public uint RecordType {
			get { return Converter.ToUInt32(this.recordType); }
		}

		public byte[] RecordData {
			get { return this.recordData; }
		}
	}

	public class PDBHead : BaseHeader
	{
		private byte[] name = new byte[32];

		private byte[] attributes = new byte[2];
		private byte[] version = new byte[2];
		private byte[] creationDate = new byte[4];
		private byte[] modificationDate = new byte[4];
		private byte[] lastBackupDate = new byte[4];
		private byte[] modificationNumber = new byte[4];
		private byte[] appInfoID = new byte[4];
		private byte[] sortInfoID = new byte[4];
		private byte[] type = new byte[4];
		private byte[] creator = new byte[4];
		private byte[] uniqueIDSeed = new byte[4];
		private byte[] nextRecordListID = new byte[4];
		private byte[] numRecords = new byte[2];
		private List<RecordInfo> recordInfoList = new List<RecordInfo>();
		private byte[] gapToData = new byte[2];

		public PDBHead()
		{
			PopulateFieldList(true);
		}

		public PDBHead(FileStream fs)
		{
			fs.Read(this.name, 0, this.name.Length);
			fs.Read(this.attributes, 0, this.attributes.Length);
			fs.Read(this.version, 0, this.version.Length);
			fs.Read(this.creationDate, 0, this.creationDate.Length);
			fs.Read(this.modificationDate, 0, this.modificationDate.Length);
			fs.Read(this.lastBackupDate, 0, this.lastBackupDate.Length);
			fs.Read(this.modificationNumber, 0, this.modificationNumber.Length);
			fs.Read(this.appInfoID, 0, this.appInfoID.Length);
			fs.Read(this.sortInfoID, 0, this.sortInfoID.Length);

			fs.Read(this.type, 0, this.type.Length);
			fs.Read(this.creator, 0, this.creator.Length);
			fs.Read(this.uniqueIDSeed, 0, this.uniqueIDSeed.Length);
			fs.Read(this.nextRecordListID, 0, this.nextRecordListID.Length);
			fs.Read(this.numRecords, 0, this.numRecords.Length);

			int recordCount = Converter.ToInt16(this.numRecords);

			for (int i = 0; i < recordCount; i++) {
				this.recordInfoList.Add(new RecordInfo(fs));
			}

			fs.Read(this.gapToData, 0, this.gapToData.Length);

			PopulateFieldList();
		}

		public string Name {
			get { return Encoding.ASCII.GetString(this.name).Replace("\0", String.Empty); }
		}

		public ushort Attributes {
			get { return Converter.ToUInt16(this.attributes); }
		}

		public ushort Version {
			get { return Converter.ToUInt16(this.version); }
		}

		public uint CreationDate {
			get { return Converter.ToUInt32(this.creationDate); }
		}

		public uint ModificationDate {
			get { return Converter.ToUInt32(this.creationDate); }
		}

		public uint LastBackupDate {
			get { return Converter.ToUInt32(this.lastBackupDate); }
		}

		public uint ModificationNumber {
			get { return Converter.ToUInt32(this.modificationNumber); }
		}

		public uint AppInfoID {
			get { return Converter.ToUInt32(this.appInfoID); }
		}

		public uint SortInfoID {
			get { return Converter.ToUInt32(this.sortInfoID); }
		}

		public uint Type {
			get { return Converter.ToUInt32(this.type); }
		}

		public uint Creator {
			get { return Converter.ToUInt32(this.creator); }
		}

		public uint UniqueIDSeed {
			get { return Converter.ToUInt32(this.uniqueIDSeed); }
		}

		public ushort NumRecords {
			get { return Converter.ToUInt16(this.numRecords); }
		}

		public ushort GapToData {
			get { return Converter.ToUInt16(this.gapToData); }
		}

		public uint MobiHeaderSize {
			get {
				if (this.recordInfoList.Count > 1) {
					return ((RecordInfo)this.recordInfoList[1]).RecordDataOffset -
					((RecordInfo)this.recordInfoList[0]).RecordDataOffset;
				} else {
					return 0;
				}
			}
		}

		public class RecordInfo
		{
			private byte[] recordDataOffset = new byte[4];
			private byte recordAttributes = 0;
			private byte[] uniqueID = new byte[3];

			public RecordInfo(FileStream fs)
			{
				fs.Read(this.recordDataOffset, 0, this.recordDataOffset.Length);
				recordAttributes = (byte)fs.ReadByte();
				fs.Read(this.uniqueID, 0, this.uniqueID.Length);
			}

			public uint RecordDataOffset {
				get { return Converter.ToUInt32(this.recordDataOffset); }
			}
		}
	}

	public class PalmDOCHead : BaseHeader
	{
		private byte[] compression = new byte[2];
		private byte[] unused0 = new byte[2];
		private byte[] textLength = new byte[4];
		private byte[] recordCount = new byte[2];
		private byte[] recordSize = new byte[2];
		private byte[] encryptionType = new byte[2];
		private byte[] unused1 = new byte[2];

		public PalmDOCHead()
		{
			PopulateFieldList(true);
		}

		public PalmDOCHead(FileStream fs)
		{
			fs.Read(this.compression, 0, this.compression.Length);
			fs.Read(this.unused0, 0, this.unused0.Length);
			fs.Read(this.textLength, 0, this.textLength.Length);
			fs.Read(this.recordCount, 0, this.recordCount.Length);

			fs.Read(this.recordSize, 0, this.recordSize.Length);
			fs.Read(this.encryptionType, 0, this.encryptionType.Length);
			fs.Read(this.unused1, 0, this.unused1.Length);

			PopulateFieldList();
		}

		//Properties
		public ushort Compression {
			get { return Converter.ToUInt16(this.compression); }
		}

		public string CompressionAsString {
			get {
				switch (this.Compression) {
					case 1:
						return "None";
					case 2:
						return "PalmDOC";
					case 17480:
						return "HUFF/CDIC";
					default:
						return String.Format("Unknown (0)", this.Compression);
				}
			}
		}

		public uint TextLength {
			get { return Converter.ToUInt32(this.textLength); }
		}

		public ushort RecordCount {
			get { return Converter.ToUInt16(this.recordCount); }
		}

		public ushort RecordSize {
			get { return Converter.ToUInt16(this.recordSize); }
		}

		public ushort EncryptionType {
			get { return Converter.ToUInt16(this.encryptionType); }
		}

		public string EncryptionTypeAsString {
			get {
				switch (this.EncryptionType) {
					case 0:
						return "None";
					case 1:
						return "Old Mobipocket";
					case 2:
						return "Mobipocket";
						;
					default:
						return String.Format("Unknown (0)", this.EncryptionType);
				}
			}
		}
	}

	public class MobiMetadata
	{
		private PDBHead pdbHeader;
		private PalmDOCHead palmDocHeader;
		private MobiHead mobiHeader;

		public PDBHead PDBHeader {
			get { return pdbHeader; }
		}

		public PalmDOCHead PalmDocHeader {
			get { return palmDocHeader; }
		}

		public MobiHead MobiHeader {
			get { return mobiHeader; }
		}

		public MobiMetadata(FileStream fs)
		{
			SetUpData(fs);
		}

		public MobiMetadata(string filePath)
		{
			System.IO.FileStream fs =
				new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			SetUpData(fs);
			fs.Close();
		}

		private void SetUpData(FileStream fs)
		{
			pdbHeader = new PDBHead(fs);
			palmDocHeader = new PalmDOCHead(fs);
			mobiHeader = new MobiHead(fs, pdbHeader.MobiHeaderSize);
		}
	}

	public class MobiHead : BaseHeader
	{
		private byte[] identifier = new byte[4];
		private byte[] headerLength = new byte[4];
		private byte[] mobiType = new byte[4];
		private byte[] textEncoding = new byte[4];
		private byte[] uniqueID = new byte[4];
		private byte[] fileVersion = new byte[4];
		private byte[] orthographicIndex = new byte[4];
		private byte[] inflectionIndex = new byte[4];
		private byte[] indexNames = new byte[4];
		private byte[] indexKeys = new byte[4];
		private byte[] extraIndex0 = new byte[4];
		private byte[] extraIndex1 = new byte[4];
		private byte[] extraIndex2 = new byte[4];
		private byte[] extraIndex3 = new byte[4];
		private byte[] extraIndex4 = new byte[4];
		private byte[] extraIndex5 = new byte[4];
		private byte[] firstNonBookIndex = new byte[4];
		private byte[] fullNameOffset = new byte[4];
		private byte[] fullNameLength = new byte[4];
		private byte[] locale = new byte[4];
		private byte[] inputLanguage = new byte[4];
		private byte[] outputLanguage = new byte[4];
		private byte[] minVersion = new byte[4];
		private byte[] firstImageIndex = new byte[4];
		private byte[] huffmanRecordOffset = new byte[4];
		private byte[] huffmanRecordCount = new byte[4];
		private byte[] huffmanTableOffset = new byte[4];
		private byte[] huffmanTableLength = new byte[4];
		private byte[] exthFlags = new byte[4];
		private byte[] restOfMobiHeader = null;
		private EXTHHead exthHeader = null;

		private byte[] remainder = null;
		private byte[] fullName = null;

		public MobiHead()
		{
			PopulateFieldList(true);
		}

		public MobiHead(FileStream fs, uint mobiHeaderSize)
		{
			fs.Read(this.identifier, 0, this.identifier.Length);

			if (this.IdentifierAsString != "MOBI") {
				throw new IOException("Did not get expected MOBI identifier");
			}

			fs.Read(this.headerLength, 0, this.headerLength.Length);
			this.restOfMobiHeader = new byte[this.HeaderLength + 16 - 132];

			fs.Read(this.mobiType, 0, this.mobiType.Length);
			fs.Read(this.textEncoding, 0, this.textEncoding.Length);
			fs.Read(this.uniqueID, 0, this.uniqueID.Length);
			fs.Read(this.fileVersion, 0, this.fileVersion.Length);
			fs.Read(this.orthographicIndex, 0, this.orthographicIndex.Length);
			fs.Read(this.inflectionIndex, 0, this.inflectionIndex.Length);
			fs.Read(this.indexNames, 0, this.indexNames.Length);
			fs.Read(this.indexKeys, 0, this.indexKeys.Length);
			fs.Read(this.extraIndex0, 0, this.extraIndex0.Length);
			fs.Read(this.extraIndex1, 0, this.extraIndex1.Length);
			fs.Read(this.extraIndex2, 0, this.extraIndex2.Length);
			fs.Read(this.extraIndex3, 0, this.extraIndex3.Length);
			fs.Read(this.extraIndex4, 0, this.extraIndex4.Length);
			fs.Read(this.extraIndex5, 0, this.extraIndex5.Length);
			fs.Read(this.firstNonBookIndex, 0, this.firstNonBookIndex.Length);
			fs.Read(this.fullNameOffset, 0, this.fullNameOffset.Length);
			fs.Read(this.fullNameLength, 0, this.fullNameLength.Length);

			int fullNameLen = Converter.ToInt32(this.fullNameLength);
			fs.Read(this.locale, 0, this.locale.Length);
			fs.Read(this.inputLanguage, 0, this.inputLanguage.Length);
			fs.Read(this.outputLanguage, 0, this.outputLanguage.Length);
			fs.Read(this.minVersion, 0, this.minVersion.Length);
			fs.Read(this.firstImageIndex, 0, this.firstImageIndex.Length);
			fs.Read(this.huffmanRecordOffset, 0, this.huffmanRecordOffset.Length);
			fs.Read(this.huffmanRecordCount, 0, this.huffmanRecordCount.Length);
			fs.Read(this.huffmanTableOffset, 0, this.huffmanTableOffset.Length);
			fs.Read(this.huffmanTableLength, 0, this.huffmanTableLength.Length);
			fs.Read(this.exthFlags, 0, this.exthFlags.Length);

			//If bit 6 (0x40) is set, then there's an EXTH record
			bool exthExists = (Converter.ToUInt32(this.exthFlags) & 0x40) != 0;

			fs.Read(this.restOfMobiHeader, 0, this.restOfMobiHeader.Length);

			if (exthExists) {
				this.exthHeader = new EXTHHead(fs);
			}

			int currentOffset = 132 + this.restOfMobiHeader.Length + ExthHeaderSize;
			this.remainder = new byte[(int)(mobiHeaderSize - currentOffset)];
			fs.Read(this.remainder, 0, this.remainder.Length);

			int fullNameIndexInRemainder = Converter.ToInt32(this.fullNameOffset) - currentOffset;

			this.fullName = new byte[fullNameLen];

			if (fullNameIndexInRemainder >= 0) {
				if (fullNameIndexInRemainder < this.remainder.Length) {
					if (fullNameIndexInRemainder + fullNameLen <= this.remainder.Length) {
						if (fullNameLen > 0) {
							Array.Copy(this.remainder,
								fullNameIndexInRemainder,
								this.fullName,
								0,
								fullNameLen);
						}
					}
				}
			}

			PopulateFieldList();
		}

		//Properties
		public int ExthHeaderSize {
			get {
				if (this.exthHeader == null) {
					return 0;
				} else {
					return this.exthHeader.Size;
				}
			}
		}

		public string FullName {
			get { return Encoding.UTF8.GetString(this.remainder).Replace("\0", String.Empty); }

            //get { return Encoding.ASCII.GetString(this.remainder).Replace("\0", String.Empty); }
		}

		public string IdentifierAsString {
			get { return Encoding.UTF8.GetString(this.identifier).Replace("\0", String.Empty); }
		}

		public uint HeaderLength {
			get { return Converter.ToUInt32(this.headerLength); }
		}

		public uint MobiType {
			get { return Converter.ToUInt32(this.mobiType); }
		}

		public string MobiTypeAsString {
			get {
				switch (this.MobiType) {
					case 2:
						return "Mobipocket Book";
					case 3:
						return "PalmDoc Book";
					case 4:
						return "Audio";
					case 257:
						return "News";
					case 258:
						return "News Feed";
					case 259:
						return "News Magazine";
					case 513:
						return "PICS";
					case 514:
						return "WORD";
					case 515:
						return "XLS";
					case 516:
						return "PPT";
					case 517:
						return "TEXT";
					case 518:
						return "HTML";
					default:
						return String.Format("Unknown (0)", this.MobiType);
				}
			}
		}

		public uint TextEncoding {
			get { return Converter.ToUInt32(this.textEncoding); }
		}

		public string TextEncodingAsString {
			get {
				switch (this.TextEncoding) {
					case 1252:
						return "Cp1252";
					case 65001:
						return "UTF-8";
					default:
						return null;
				}
			}
		}

		public uint UniqueID {
			get { return Converter.ToUInt32(this.uniqueID); }
		}

		public uint FileVersion {
			get { return Converter.ToUInt32(this.fileVersion); }
		}

		public uint OrthographicIndex {
			get { return Converter.ToUInt32(this.orthographicIndex); }
		}

		public uint InflectionIndex {
			get { return Converter.ToUInt32(this.inflectionIndex); }
		}

		public uint IndexNames {
			get { return Converter.ToUInt32(this.indexNames); }
		}

		public uint IndexKeys {
			get { return Converter.ToUInt32(this.indexKeys); }
		}

		public uint ExtraIndex0 {
			get { return Converter.ToUInt32(this.extraIndex0); }
		}

		public uint ExtraIndex1 {
			get { return Converter.ToUInt32(this.extraIndex1); }
		}

		public uint ExtraIndex2 {
			get { return Converter.ToUInt32(this.extraIndex2); }
		}

		public uint ExtraIndex3 {
			get { return Converter.ToUInt32(this.extraIndex3); }
		}

		public uint ExtraIndex4 {
			get { return Converter.ToUInt32(this.extraIndex4); }
		}

		public uint ExtraIndex5 {
			get { return Converter.ToUInt32(this.extraIndex5); }
		}

		public uint FirstNonBookIndex {
			get { return Converter.ToUInt32(this.firstNonBookIndex); }
		}

		public uint FullNameOffset {
			get { return Converter.ToUInt32(this.fullNameOffset); }
		}

		public uint FullNameLength {
			get { return Converter.ToUInt32(this.fullNameLength); }
		}

		public uint MinVersion {
			get { return Converter.ToUInt32(this.minVersion); }
		}

		public uint HuffmanRecordOffset {
			get { return Converter.ToUInt32(this.huffmanRecordOffset); }
		}

		public uint HuffmanRecordCount {
			get { return Converter.ToUInt32(this.huffmanRecordCount); }
		}

		public uint HuffmanTableOffset {
			get { return Converter.ToUInt32(this.huffmanTableOffset); }
		}

		public uint HuffmanTableLength {
			get { return Converter.ToUInt32(this.huffmanTableLength); }
		}

		public EXTHHead EXTHHeader {
			get { return this.exthHeader; }
		}
	}
}
