   M S B A M L   `   `   `  ����C  ?wondaRew, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:wondaRewwondaRew  5   	       wondaRew.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:wondaRew     5   	   -   5          �Name$  firstMainWindow��5   	    � Title$ 
MainWindow��5   	   $	��450��6   $	��400��6)    � 
ResizeMode$ NoResize=�65    � WindowStartupLocation$ CenterScreen=�6K    � Icon$ /favicon.ico��6p   .��5	      � $��Center=�6   $��12q�6&   .+�5
   
   �� -   5         ��Name$ hashingPanel��5
      $y�Hidden=�6+   .+�5      �� $
��Info��6   $��24(�6#   $��Center=�61   �� 5      -   5         ��Name$ hashingPanel_filename��5   '   $��FileName��6   6'   �� 5      -   5        $ hashingPanel_filesize��5   '   $��FileSize��6   6'   �� 5      $��Visible=�6   $��Hidden=�6C   .��5      �� -   5        $ hashingPanel_hashes��5   .   $��FileHashing��6   6.   5      5      �� 5   
   -   5        $ encrypt_decryptPanel��5      $y�Visible=�63   .+�5      �� -   5        $  encrypt_decryptPanel_title��5   &   $��Encrypt��6   $��24(�6J   $��Center=�6X   �� 5      $��Type a password��6   �� 5      -   5         ��Name$# encrypt_decryptPanel_password��5      $�� ��6=   
�������$
��1,12q�6\   $��28��6j    �� TextAlignment$ Center=�6v   �� 5      -	   5        	 ��Name$!	 encrypt_decryptPanel_button��5      $�� ��6:   $��45��6E   
 � Cursor$

 Hand��6Q   5      5      5      