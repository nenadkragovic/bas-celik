﻿using BashChelik.DataModel;
using BashChelik.Native;
using PCSC;

namespace BashChelik
{


    public static class ElectronicIdReader
    {


        public static ElectronicIdentificationData ReadAll()
        {

            // Create PC/SC context
            string[] readerNames;
            using (var context = new SCardContext())
            {
                context.Establish(SCardScope.System);
                readerNames = context.GetReaders();

                if (readerNames.Length > 0)
                {
                    return ReadAll(readerNames[0]);
                }
                //TODO:Handle this better, maybe throw custom Exception
                else return null;
            }

        }

        public static ElectronicIdentificationData ReadAll(string readerName, int apiVersion = 3)
        {
            using (var advancedReader = new AdvancedReader(apiVersion))
            {
                int cardType = 2;
                var nativeResult = NativeMethods.EidBeginRead(readerName, out cardType);
                AdvancedReader.CheckNativeResult(nativeResult);
                ElectronicIdentificationData result = new ElectronicIdentificationData();
                result.CardType = (CardType)cardType;
                result.DocumentData = advancedReader.ReadDocumentData();
                result.FixedPersonalData = advancedReader.ReadFixedPersonalData();
                result.VariablePersonalData = advancedReader.ReadVariablePersonalData();
                //result.PortraitData = advancedReader.ReadPortraitData();
                result.CertificateData = advancedReader.ReadCertificateData(result.CardType);
                return result;
            }
        }
    }
}
