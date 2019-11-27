using System;
using System.Text;
namespace Common
{
	public class UrlDecoder {
            private int _bufferSize;
 
            // Accumulate characters in a special array
            private int _numChars;
            private char[] _charBuffer;
 
            // Accumulate bytes for decoding into characters in a special array
            private int _numBytes;
            private byte[] _byteBuffer;
 
            // Encoding to convert chars to bytes
            private Encoding _encoding;
 
            private void FlushBytes() {
                if (_numBytes > 0) {
                    _numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
                    _numBytes = 0;
                }
            }
 
            internal UrlDecoder(int bufferSize, Encoding encoding) {
                _bufferSize = bufferSize;
                _encoding = encoding;
 
                _charBuffer = new char[bufferSize];
                // byte buffer created on demand
            }
 
            internal void AddChar(char ch) {
                if (_numBytes > 0)
                    FlushBytes();
 
                _charBuffer[_numChars++] = ch;
            }
 
            internal void AddByte(byte b) {
                // if there are no pending bytes treat 7 bit bytes as characters
                // this optimization is temp disable as it doesn't work for some encodings
                /*
                                if (_numBytes == 0 && ((b & 0x80) == 0)) {
                                    AddChar((char)b);
                                }
                                else
                */
                {
                    if (_byteBuffer == null)
                        _byteBuffer = new byte[_bufferSize];
 
                    _byteBuffer[_numBytes++] = b;
                }
            }
 
            internal String GetString() {
                if (_numBytes > 0)
                    FlushBytes();
 
                if (_numChars > 0)
                    return new String(_charBuffer, 0, _numChars);
                else
                    return String.Empty;
            }
        }
}