using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Parrot.Cryptography
{
    public class RSACryption
    {
        // Methods
        public bool GetHash(FileStream objFile, ref string strHashData)
        {
            byte[] inArray = HashAlgorithm.Create("MD5").ComputeHash(objFile);
            objFile.Close();
            strHashData = Convert.ToBase64String(inArray);
            return true;
        }

        public bool GetHash(FileStream objFile, ref byte[] HashData)
        {
            HashData = HashAlgorithm.Create("MD5").ComputeHash(objFile);
            objFile.Close();
            return true;
        }

        public bool GetHash(string m_strSource, ref string strHashData)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("MD5");
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            byte[] inArray = algorithm.ComputeHash(bytes);
            strHashData = Convert.ToBase64String(inArray);
            return true;
        }

        public bool GetHash(string m_strSource, ref byte[] HashData)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("MD5");
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = algorithm.ComputeHash(bytes);
            return true;
        }

        public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);
            byte[] bytes = provider.Decrypt(DecryptString, false);
            return new UnicodeEncoding().GetString(bytes);
        }

        public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);
            byte[] rgb = Convert.FromBase64String(m_strDecryptString);
            byte[] bytes = provider.Decrypt(rgb, false);
            return new UnicodeEncoding().GetString(bytes);
        }

        public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPublicKey);
            byte[] bytes = new UnicodeEncoding().GetBytes(m_strEncryptString);
            return Convert.ToBase64String(provider.Encrypt(bytes, false));
        }

        public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPublicKey);
            return Convert.ToBase64String(provider.Encrypt(EncryptString, false));
        }

        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            xmlKeys = provider.ToXmlString(true);
            xmlPublicKey = provider.ToXmlString(false);
        }

        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
        {
            byte[] rgbHash = Convert.FromBase64String(p_strHashbyteDeformatter);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPublic);
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm("MD5");
            byte[] rgbSignature = Convert.FromBase64String(p_strDeformatterData);
            return deformatter.VerifySignature(rgbHash, rgbSignature);
        }

        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
        {
            byte[] rgbHash = Convert.FromBase64String(p_strHashbyteDeformatter);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPublic);
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm("MD5");
            return deformatter.VerifySignature(rgbHash, DeformatterData);
        }

        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPublic);
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm("MD5");
            byte[] rgbSignature = Convert.FromBase64String(p_strDeformatterData);
            return deformatter.VerifySignature(HashbyteDeformatter, rgbSignature);
        }

        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPublic);
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm("MD5");
            return deformatter.VerifySignature(HashbyteDeformatter, DeformatterData);
        }

        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPrivate);
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("MD5");
            byte[] inArray = formatter.CreateSignature(HashbyteSignature);
            m_strEncryptedSignatureData = Convert.ToBase64String(inArray);
            return true;
        }

        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            byte[] rgbHash = Convert.FromBase64String(m_strHashbyteSignature);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPrivate);
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("MD5");
            EncryptedSignatureData = formatter.CreateSignature(rgbHash);
            return true;
        }

        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
        {
            byte[] rgbHash = Convert.FromBase64String(m_strHashbyteSignature);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPrivate);
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("MD5");
            byte[] inArray = formatter.CreateSignature(rgbHash);
            m_strEncryptedSignatureData = Convert.ToBase64String(inArray);
            return true;
        }

        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPrivate);
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("MD5");
            EncryptedSignatureData = formatter.CreateSignature(HashbyteSignature);
            return true;
        }
    }
}