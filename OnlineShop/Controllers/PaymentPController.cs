using Newtonsoft.Json;
using OnlineShop.DAL;
using OnlineShop.Models;
using OnlineShop.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace OnlineShop.Controllers
{
    public class PaymentPController : Controller
    {
        private const int RunningOrderNumber = 301;
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();


        // GET: PaymentP
        public ActionResult PaymentP(int? total)
        {
            // Use the total price in your logic
            ViewBag.Total = total+ 3.54;
            return View();
        }

        [HttpPost]
        public ActionResult SaveCardDetails(string cardNumber, string cardType, string cvcCode, DateTime expirationDate, Tbl_CardDetails tbl)
        {
            if (!string.IsNullOrEmpty(cardNumber))
            {
                try
                {
                    string encryptedCardNumber = EncryptionHelper.Encrypt(cardNumber);
                    string encryptedCvcCode = EncryptionHelper.Encrypt(cvcCode);

                    tbl.CvCCode = encryptedCvcCode;
                    tbl.cardType = cardType;
                    tbl.CardNumber = encryptedCardNumber;
                    tbl.ExpirationDate = expirationDate;


                    _unitOfWork.GetRepositoryInstance<Tbl_CardDetails>().Add(tbl);
                    _unitOfWork.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("An error occurred: " + ex.Message);
                    Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                    return Json(new { success = false, message = "An error occurred while saving card details." });
                }
            }

            // If card number is empty, return error
            return Json(new { success = false, message = "Card number is required." });
        }




        public static class EncryptionHelper
        {
            private const string Key = "e6S4F8u1I2G3c7p9"; // Replace with your encryption key

            public static string Encrypt(string plainText)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                    aesAlg.IV = new byte[16]; // Use a unique IV for each encryption

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }

            public static string Decrypt(string cipherText)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                    aesAlg.IV = new byte[16]; // Use a unique IV for each encryption

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    byte[] cipherBytes = Convert.FromBase64String(cipherText);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

    }
}






