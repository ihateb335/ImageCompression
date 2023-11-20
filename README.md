# ImageCompression subject

Contains of:

* ImageCompression class library:
  * [FileCompressor](CompressionLibrary/FileCompressor.cs) - abstract class for both compressor and decompressor;
  ---
  * [RLECompressor](CompressionLibrary/RLE/RLECompressor.cs) - class for RLE compression;
  * [RLEDecompressor](CompressionLibrary/RLE/RLEDecompressor.cs) - class for RLE decompression;
  ---
  * [RLEICompressor](CompressionLibrary/RLEI/RLEICompressor.cs) - class for RLE improved compression;
  * [RLEIDecompressor](CompressionLibrary/RLEI/RLEIDecompressor.cs) - class for RLE improved decompression;
  ---
  * [LZWCompressor](CompressionLibrary/LZW/LZWCompressor.cs) - class for LZW compression [Beware of low performance];
  * [LZWDecompressor](CompressionLibrary/LZW/LZWDecompressor.cs) - class for LZW decompression [Beware of low performance];
  ---
  * [HuffmanCompressor](CompressionLibrary/Huffman/HuffmanCompressor.cs) - class for Huffman compression [Beware of low performance];
  * [HuffmanDecompressor](CompressionLibrary/Huffman/HuffmanDecompressor.cs) - class for Huffman decompression [Beware of low performance];
  ---
  * [BitReader](CompressionLibrary/BitOperations/BitReader.cs) - class to read bits of data;
  * [BitWriter](CompressionLibrary/BitOperations/BitWriter.cs) - class to write bits of data;
  ---
  * [CodeTable](CompressionLibrary/LZW/CodeTable.cs) - abstract class for the code tables;
  * [WriteCodeTable](CompressionLibrary/LZW/WriteCodeTable.cs) - class for the write code table;
  * [ReadCodeTable](CompressionLibrary/LZW/ReadCodeTable.cs) - class for the read code table;
    
* ImageCompression Programs:
  * [RLE](CompressionPresentation/CompressionPresenter.cs) - program containing all the algorithms;
* ImageCompression Tests:
  * [RLE](CompressionLibraryTesting/RLETests.cs) - tests for RLE algorithm;
  * [RLEI](CompressionLibraryTesting/RLEITests.cs) - tests for RLE improved algorithm;
  * [LZW](CompressionLibraryTesting/LZWTests.cs) - tests for basic LZW algorithm;
  * [LZWI](CompressionLibraryTesting/LZWITests.cs) - tests for LZW improved algorithm;
  * [HuffmanIntegration](CompressionLibraryTesting/HuffmanCompressionTests.cs) - Huffman integration tests;
  * [Huffman](CompressionLibraryTesting/HuffmanTests.cs) - tests for Huffman;
  ---
  * [BitReaderTests](CompressionLibraryTesting/BitReaderTests.cs) - tests for bit reader;
  * [BitWriterTests](CompressionLibraryTesting/BitWriterTests.cs) - tests for bit writer;
  * [BitIntegration](CompressionLibraryTesting/BitIntegration.cs) - tests for both bit reader and writer();
