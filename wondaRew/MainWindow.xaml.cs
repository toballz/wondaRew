
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using toolsHelper;


namespace wondaRew
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool fileExistOrDie(string filePath1)
        {
            if (!File.Exists(filePath1))
            {
                MessageBox.Show("This file does not exist.", filePath1??"No File really !"); Environment.Exit(0);
                return false;
            }
            return true;
        }
        private void navigatePanel(StackPanel? panrl)
        {
            hashingPanel.Visibility = Visibility.Hidden;
            encrypt_decryptPanel.Visibility = Visibility.Hidden;
            if (panrl != null) panrl.Visibility = Visibility.Visible; 
        }
        private void shreddingClicked()
        {
            navigatePanel(null);
            fileExistOrDie(Tool.FilePathToWorkOn!);
            var result = MessageBox.Show(Tool.FilePathToWorkOn, "Do you want to \"Shred this file\"", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    h.Shred_File(Tool.FilePathToWorkOn!);
                }
                catch (Exception j) { MessageBox.Show(j.Message); }
            }
            Thread.Sleep(10);
            Environment.Exit(0);
        }
        private void hashesClicked()
        {
            navigatePanel(hashingPanel);
            fileExistOrDie(Tool.FilePathToWorkOn!);
            try
            {
                // Read all bytes from the file
                byte[] fileBytes = File.ReadAllBytes(Tool.FilePathToWorkOn!);
                String woolHash = "";

                hashingPanel_filename.Content = "File Name: "+Path.GetFileName(Tool.FilePathToWorkOn!);
                hashingPanel_filesize.Content = "File Size: "+fileBytes.Length.ToString("N0") + " bytes \n\t" 
                                                             + ( fileBytes.Length / 1024f).ToString("N") + " KB \n\t" 
                                                             +  (fileBytes.Length / (1024f*1024f)).ToString("N")  + " MB";

                // Compute SHA-256 hash  
                woolHash = "\r\n\nSHA256: \t" + h.Hash.SHA256_(fileBytes);
                // Compute SHA-384 hash 
                woolHash += "\r\n\nSHA384: \t" + h.Hash.SHA384_(fileBytes);
                // Compute MD5 hash  
                woolHash += "\r\n\nMD5: \t" + h.Hash.MD5_(fileBytes);
                // Compute SHA-1 hash 
                woolHash += "\r\n\nSHA-1: \t" + h.Hash.SHA1_(fileBytes);
                // Compute SHA-512 hash 
                woolHash += "\r\n\nSHA512: " + h.Hash.SHA512_(fileBytes);

                hashingPanel_hashes.Content = woolHash;

            }
            catch (IOException eq)
            {
                MessageBox.Show($"Error reading the file: {eq.Message}");
            }
            catch (UnauthorizedAccessException ew)
            {
                MessageBox.Show($"Unauthorized access: {ew.Message}");
            }
            catch (Exception ee)
            {
                MessageBox.Show($"Error: {ee.Message}");
            }
        }
        private void encrypt_decryptClicked(int rt)
        {
            if (rt == 1)
            { 
                encrypt_decryptPanel_title.Content = "Encrypt";
                encrypt_decryptPanel_button.Content = "Encrypt";
                encrypt_decryptPanel_button.Click += (object a, RoutedEventArgs e)=>
                {
                    if (encrypt_decryptPanel_password.Text.Length > 4)
                    {
                        string outTempFile = Path.GetTempFileName();
                        string? infilePath = Path.GetDirectoryName(Tool.FilePathToWorkOn);
                        string? infileName = Path.GetFileName(Tool.FilePathToWorkOn);

                        try
                        {
                            string? encryptionResult = C.EncryptByteStringorAnything_Aes(Tool.FilePathToWorkOn!, outTempFile, 
                                                                encrypt_decryptPanel_password.Text,
                                                                Tool.encryption_pwdSalt_1, 
                                                                Tool.encryption_pwdSalt_2);
                            if (encryptionResult == null) {
                                File.Move(outTempFile, Path.Combine(infilePath!, infileName + Tool.encryption_outFileExtension));
                                h.Shred_File(Tool.FilePathToWorkOn!);
                            }
                            else
                            {
                                MessageBox.Show(encryptionResult);
                            }
                            Environment.Exit(0);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); Environment.Exit(0); }
                    }
                    else { MessageBox.Show("Password should be greater than 5 characters."); Environment.Exit(0); }
                };


            }
            else if(rt == 0)
            {
                

                        encrypt_decryptPanel_title.Content = "Decrypt";
                        encrypt_decryptPanel_button.Content = "Decrypt";
                        encrypt_decryptPanel_button.Click += (object a, RoutedEventArgs e) =>
                        {
                            if (encrypt_decryptPanel_password.Text.Length > 4)
                            {
                                try
                                {
                                    if (!Path.GetExtension(Tool.FilePathToWorkOn!).Equals(Tool.encryption_outFileExtension, StringComparison.OrdinalIgnoreCase))
                                    {
                                        MessageBox.Show("File Cannot be decrypted");
                                        Environment.Exit(0);
                                    }
                                    string outTempFile = Path.GetTempFileName();
                                    string? infilePath = Path.GetDirectoryName(Tool.FilePathToWorkOn);
                                    string? infileName_WithoutExtension = Path.GetFileNameWithoutExtension(Tool.FilePathToWorkOn);

                                    string? decryptionResult = C.DecryptByteStringorAnything_Aes(Tool.FilePathToWorkOn!, outTempFile,
                                                                        encrypt_decryptPanel_password.Text,
                                                                        Tool.encryption_pwdSalt_1,
                                                                        Tool.encryption_pwdSalt_2);
                                    if (decryptionResult == null) { 
                                        File.Move(outTempFile, Path.Combine(infilePath!, infileName_WithoutExtension!));
                                        h.Shred_File(Tool.FilePathToWorkOn); 
                                    }
                                    else
                                    {
                                        h.Shred_File(outTempFile);
                                        MessageBox.Show(decryptionResult);
                                    }
                                    Environment.Exit(0);
                                }
                                catch (Exception ex) { MessageBox.Show(ex.Message); Environment.Exit(0); }
                            }
                            else { MessageBox.Show("password less than 5."); Environment.Exit(0); }
                        };
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            onstart(Environment.GetCommandLineArgs());
        }

        private void onstart(string[] args)
        {
            //add to registry
            string xba = Assembly.GetExecutingAssembly().Location;
            string thisFileFullPath = Path.Combine(Environment.CurrentDirectory, Process.GetCurrentProcess().ProcessName+".exe");
            if (File.Exists(thisFileFullPath))
            {
                //MessageBox.Show(thisFileFullPath);
                Tool.RegisterContextMenuIfAdmin(Tool.LadderName, Tool.LadderDescription, thisFileFullPath, Tool.LadderChildren);
            }

            //
            //
            //get args
            var parsedArgs = h.ParseArguments(args);
            if (parsedArgs.ContainsKey("--file") && parsedArgs["--file"] != "")
            {
                Tool.FilePathToWorkOn = parsedArgs["--file"];

                fileExistOrDie(Tool.FilePathToWorkOn!);
                if (parsedArgs.ContainsKey("--command") && parsedArgs["--command"] != "")
                {
                    fileExistOrDie(Tool.FilePathToWorkOn);
                    var command = parsedArgs["--command"];
                    if (command == "hash")
                    {
                        firstMainWindow.ResizeMode=ResizeMode.CanResize;
                        firstMainWindow.Width = 900;
                        hashesClicked();
                    }
                    else if (command == "shred")
                    {
                        shreddingClicked();
                    }
                    else if (command == "decrypt")
                    {
                        navigatePanel(encrypt_decryptPanel);
                        encrypt_decryptClicked(0);
                    }
                    else if (command == "encrypt")
                    {
                        navigatePanel(encrypt_decryptPanel);
                        encrypt_decryptClicked(1);
                    }
                }
                else
                {
                    MessageBox.Show("Wrong command."); Environment.Exit(0);
                }
            }
            else
            {
                MessageBox.Show("what type of file command."); Environment.Exit(0);
            }



        }


    }
}
