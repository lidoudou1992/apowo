using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public interface IProtoObject
{
    void WriteTo(Stream stream);
    void ReadFrom(Stream stream);

    void ReadFrom(byte[] bytes, int index, int count);
}
