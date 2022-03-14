using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;
using Windows.ApplicationModel.Activation;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;

namespace GDCortanaCommands
{

    class CortanaFunctions
    {
        public static HostName host = new HostName("172.17.30.37");
        public static string port = ("27994");

        /*
        This is the lookup of VCD CommandNames as defined in 
        CustomVoiceCommandDefinitios.xml to their corresponding actions
        */
        private readonly static IReadOnlyDictionary<string, Delegate> vcdLookup = new Dictionary<string, Delegate>{

            /*
            {<command name from VCD>, (Action)(async () => {
                 <code that runs when that commmand is called>
            })}
            */

			{"Connect", (Action)(async () => {

                        StreamSocket socket = new StreamSocket();
                        socket.Control.KeepAlive = false;

                        try
                        {
                            await socket.ConnectAsync(host, port);
                            Stream streamOut = socket.OutputStream.AsStreamForWrite();
                            StreamWriter writer = new StreamWriter(streamOut);
                            string request = "<root><connect><DviConsole type=\"name\">DVI-CON</DviConsole><DviCpu type=\"name\">HDM-CPU</DviCpu><CloseDialogs /></connect></root>";
                            await writer.WriteLineAsync(request);
                            await writer.FlushAsync();

                            // Code for reading
                            // Stream streamIn = socket.OutputStream.AsStreamForRead();
                            // StreamReader reader = new StreamReader(streamIn);
                            // char[] result = new char[reader.BaseStream.Length];
                            // await reader.ReadAsync(result, 0, (int)reader.BaseStream.Length);

                            // MessageDialog popup = new MessageDialog(reader);
                            // await popup.ShowAsync();

                            // Your data will be in results
                            socket.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Debug.Write("Fail" + ex.Message);
                        }
            })},

    };

        /*
        Register Custom Cortana Commands from VCD file
        */
        public static async void RegisterVCD()
        {
            try
            {
      
            StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync(@"GDVoiceCommandDefinitions.xml");

            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);
            }
            catch (Exception ex)
            {
                Debug.Write("Fail" + ex.Message);
            }
        }

        /*
        Look up the spoken command and execute its corresponding action
        */
        public static void RunCommand(VoiceCommandActivatedEventArgs cmd)
        {
            SpeechRecognitionResult result = cmd.Result;
            string commandName = result.RulePath[0];
            vcdLookup[commandName].DynamicInvoke();
        }
        public static void CloseApp()
        {
            CoreApplication.Exit();
        }
    }
}
