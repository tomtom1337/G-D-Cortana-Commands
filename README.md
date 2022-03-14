# G-D-Cortana-Commands Template for G&D Devices
This is a simple template for how to register G&D IP-Control-API commands with Cortana on Windows 10.

Adopted and modified from: https://github.com/crclayton/custom-cortana-commands-template

# Quick Start Guide
- Setup the command prefix in `GDVoiceCommandDefinitions.xml`
```
<CommandPrefix> Matrix </CommandPrefix>
```

- Register a voice command in `GDVoiceCommandDefinitions.xml`
```
	<Command Name="Connect">
		<Example> Connect CON to CPU </Example>
		<ListenFor> Connect DVI-CON </ListenFor>
		<Feedback> DONE </Feedback>
		<Navigate/>
	</Command>
```
- Enter code that runs when that commmand is called in `CortanaFunctions.cs` 

```

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

```
- Say: Hey Cortana + Prefix + Command

```
Hey Cortana - Matrix - Connect DVI-CON
```

# About

## Contributing
For changes or issues, please open an [request](https://github.com/tomvalk/G-D-Cortana-Commands/issues) first to discuss what you would like to change. <br/>
		

## 
<a href="https://www.buymeacoffee.com/tomtom1337" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>
