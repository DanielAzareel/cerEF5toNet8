using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace BusinessModel.Business
{
    public class MetodosGenericosBL
    {

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                ///if (prop.ComponentType.IsSealed) {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                /// }
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static Boolean ValidarAlfanumerico(string texto)
        {
            Regex reg = new Regex(@"^([a-zA-ZZáéíóúÁÉÍÓÚÑñüÜ0-9 .]+)$");
            if (reg.IsMatch(texto))
            {
                return true;
            }
            return false;

        }

        public static byte[] httpBaseToByte(HttpPostedFileBase file)
        {
            byte[] data;
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

            return data;
        }

        public byte[] GenerarQr(string cadenaQR)
        {
            try
            {
                var qrEncoder = new QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.H);
                var qrCode = qrEncoder.Encode(cadenaQR);


                var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

                var streamFile = new MemoryStream();
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, streamFile);
                return streamFile.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string FirstCharToUpper(string input)
        {
            try
            {
                switch (input)
                {
                    case null: return "";
                    case "": return "";
                    default: return input.Substring(0, 1).ToString().ToUpper() + input.Substring(1).ToLower();
                }
            }
            catch (Exception ex)
            {

                return ""; 
            }

        }

    }
}
