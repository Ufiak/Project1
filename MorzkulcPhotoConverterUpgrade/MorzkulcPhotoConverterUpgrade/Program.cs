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
            string input;


            Console.WriteLine("            Welcome to this amazing \n                Photo Converter! \n               Let's get started!\n");
            
            loadingXML = OtherMethodsContainer.CheckForXmlConfig();
  
            ImageMetaDataContainer myContainer;
            OtherMethodsContainer source;

            if (loadingXML.Equals("yes"))
            {
                string xmlFilePath = "XML file\\PhotoConverterCommands.xml";
                myContainer = new ImageMetaDataContainer(xmlFilePath); // !!!!!!!!!!!!!!!! QUESTION to Filip. with no return type in the ImageMetaDataContainer, how it works?
            }
            else
            {
                input = OtherMethodsContainer.Input();
                if (input.Equals("single"))
                {
                    source = new OtherMethodsContainer.SourceMethod(inputSingle); // it would recqire a whole new class...
                    // poczytac na tutorialspoint.com/csharp/csharp_methods.html
                }
                else if (input.Equals("multiple") || input.Equals("multi"))
                {
                    source = new OtherMethodsContainer.SourceMethod(inputMultiple);
                }
                else



                    Tuple<string, string, string> getValueOfSourceMethod = OtherMethodsContainer.SourceMethod(); // this thing will initialise SourceMethod once and will enable to get the values of it for following variables.
                //getValueOfSourceMethod = new OtherMethodsContainer()

                Console.WriteLine("\nProvide value to height or with to resize image with original aspect ratio for this size. \nOther value write '0' or press enter. \nWrite both values to loose aspect ratio but receive the demanded values");
                width = OtherMethodsContainer.WidthMethod();
                height = OtherMethodsContainer.HeightMethod();
                format = OtherMethodsContainer.FormatMethodCheck();

                Tuple<string, string, string> getValueOfDestinationMethod = OtherMethodsContainer.DestinationMethod(format);

                myContainer = new ImageMetaDataContainer(getValueOfSourceMethod, width, height, format, getValueOfDestinationMethod);
            }

            string joinedformatOfFileInRootFolder = "*.jpg|*.png|*.gif|*.tiff";
            string[] files = OtherMethodsContainer.GetFiles(myContainer.sourceOfFolder, joinedformatOfFileInRootFolder,SearchOption.AllDirectories);


            PhotoConversionContainer.ImageConversionMethod(myContainer, files);


            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}