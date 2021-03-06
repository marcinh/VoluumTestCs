﻿namespace VoluumTestCs.Framework.SerialObjects
{
    /// <summary>
    /// Mock object for JSON serializtion
    /// </summary>
    public class CountryObject
    {
        public string code { get; private set; }

        public CountryObject(string code)
        {
            this.code = code;
        }

        public static CountryObject Poland { get { return new CountryObject("PL"); } }
    }
}
