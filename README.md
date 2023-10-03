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
    
* ImageCompression Programs:
  * [RLE](Lab_01/Lab_01.cs) - program using RLE algorithm;
  * [RLEI](Lab_01I/Lab_01I.cs) - program using RLE improved algorithm;
* ImageCompression Tests:
  * [RLE](CompressionLibraryTesting/RLETests.cs) - tests for RLE algorithm;
  * [RLE_Data](CompressionLibraryTesting/data/RLEData.cs) - Data for the tests of RLE algorithm;
  ---
  * [RLEI](CompressionLibraryTesting/RLEITests.cs) - tests for RLE improved algorithm;
  * [RLEI_Data](CompressionLibraryTesting/data/RLEIData.cs) - Data for the tests of RLE improved algorithm;
