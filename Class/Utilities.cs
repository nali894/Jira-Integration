namespace BusinessLayer.Jira.Integration
{
    internal class Utilities
    {
        /// <summary>
        /// Codificar texto en formato base 64
        /// </summary>
        /// <param name="strToEncode">Texto que se codificará</param>
        /// <returns></returns>
        static internal string EncodeTo64(string strToEncode)
        {

            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strToEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
