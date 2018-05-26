using System;
using System.IO; 


namespace MorzkulcPhotoConverterUpgrade
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 0;
            int height = 0;
            string format = "";
            string loadingXML;


            Console.WriteLine("            Welcome to this amazing \n                Photo Converter! \n               Let's get started!\n");
            
            loadingXML = InputContainer.CheckForXmlConfig();
  
            ImageMetaDataContainer myContainer;

            if (loadingXML.Equals("yes"))
            {
                string xmlFilePath = "XML file\\PhotoConverterCommands.xml";
                myContainer = new ImageMetaDataContainer(xmlFilePath); 
            }
            else
            {
                Tuple<string, string, string> getValueOfSourceMethod = InputContainer.SourceMethod();

                Console.WriteLine("\nProvide value to height or with to resize image with original aspect ratio for this size. \nOther value write '0' or press enter. \nWrite both values to loose aspect ratio but receive the demanded values");
                width = InputContainer.Width();
                height = InputContainer.Height();
                format = InputContainer.FormatCheck();

                Tuple<string, string, string> getValueOfDestinationMethod = InputContainer.Destination(format);

                myContainer = new ImageMetaDataContainer(getValueOfSourceMethod, width, height, format, getValueOfDestinationMethod);
            }

            string joinedformatOfFileInRootFolder = "*.jpg|*.png|*.gif|*.tiff";
            string[] files = InputContainer.GetFiles(myContainer.sourceOfFolder, joinedformatOfFileInRootFolder,SearchOption.AllDirectories);


            PhotoConversionContainer.ImageConversion(myContainer, files);


            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}