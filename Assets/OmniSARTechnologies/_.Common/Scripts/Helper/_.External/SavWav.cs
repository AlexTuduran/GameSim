//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
//  derived from Gregorio Zanon's script
//  http://forum.unity3d.com/threads/119295-Writing-AudioListener.GetOutputData-to-wav-problem?p=806734&viewfull=1#post806734

using System;
using System.IO;
using UnityEngine;

public static class SavWav {
    private const int HeaderSize = 44;

    public static bool Save(string filePath, string fileName, AudioClip audioClip) {
        fileName = Path.Combine(filePath, fileName);
        return Save(fileName, audioClip);
    }

    public static bool Save(string fileName, AudioClip audioClip) {
        if (!fileName.ToLower().EndsWith(".wav")) {
            fileName += ".wav";
        }

        Debug.Log(fileName);

        // make sure directory exists if user is saving to sub dir
        var path = Path.GetDirectoryName(fileName);

        if (path == null) {
            return false;
        }

        Directory.CreateDirectory(path);

        using (var fileStream = CreateEmpty(fileName)) {
            ConvertAndWrite(fileStream, audioClip);
            WriteHeader(fileStream, audioClip);
        }

        if (!File.Exists(fileName)) {
            return false;
        }

        return true;
    }

    public static bool SaveToApplicationPersistentDataPath(string filename, AudioClip audioClip) {
        return Save(Application.persistentDataPath, filename, audioClip);
    }

    private static FileStream CreateEmpty(string filePath) {
        var fileStream = new FileStream(filePath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < HeaderSize; i++) { //preparing the header
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    private static void ConvertAndWrite(FileStream fileStream, AudioClip audioClip) {
        var samples = new float[audioClip.samples];
        audioClip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++) {
            intData[i] = (short)(Mathf.Clamp(samples[i], -1.0f, +1.0f) * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }

    private static void WriteHeader(FileStream fileStream, AudioClip audioClip) {
        var hz = audioClip.frequency;
        var channels = audioClip.channels;
        var samples = audioClip.samples;

        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2);
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);

		fileStream.Close();
    }
}