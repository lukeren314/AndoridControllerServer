   M S B A M L   `   `   `  ����L  HAndroidController, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AndroidControllerAndroidController  5   	   "    AndroidController.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:AndroidController     5   	   -   5          � Title$  AndroidControllerServer��5   	   $	��450��6)   $	��800��66   .��5
      � .+�5   
   �� $��Android Controller Server��6   $
��Left=�64   $��36��6O   $��317,21,0,364q�6[   $��Center=�6q   $	��168��6�   �� 5   
   -   5         ��Name$ ServerStatusButton��5      $��Start Server��6-   $
��Left=�6e   $��23��6�   $��
57,104,0,0q�6�   $	��Top=�6�   $��81��6�   �� 5   
   -   5         ��Name$ ServerStatusLabel��5      $��Server Inactive��6+   $
��Left=�6E   $��23��6`   $��138,104,0,0q�6l   $	��Top=�6�   $��89��6�   �� 5   
   $��Available Virtual Devices��6   $
��Left=�64   $��
583,21,0,0q�6O   $	��Top=�6c   $��27��6{   $	��140��6�   �� 5   
   -   5         ��Name$ DevicesListBox��5      $
��Left=�6*   $��52��6E   $��
583,48,0,0q�6Q   $	��Top=�6e   $	��140��6}   �� 5   
   -   5        $ VJoyStatusLabel��5      $��vJoy Disabled��6)   $
��Left=�6A   $��	57,44,0,0q�6\   $	��Top=�6o   $	��170��6�   �� 5   
   -   5         ��Name$ IpAddressTextBox��5      $
��Left=�6,   $��20��6G   $��
374,73,0,0q�6S    �� TextWrapping$
 Wrap=�6g   $	��Top=�6{   $	��120��6�    �� IsReadOnlyCaretVisible . 6�   �� 5   
   $��Port:��6   $
��Left=�6    $��28��6;   $��
300,98,0,0q�6G   $	��Top=�6[   $��69��6s   �� 5   
   -   5        $ PortTextBox��5      $
��Left=�6'   $��23��6B   $
 Wrap=�6N   $�� ��6b   $	��Top=�6j   $	��120��6�   $��374,104,0,0q�6�   �� 5   
   $��	Devices
��6   $
��Left=�6,   $��
57,135,0,0q�6G   $	��Top=�6[   $��25��6s   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 
SocketGrid��5      $
��Left=�6'   $	��218��6B   $��
57,160,0,0q�6O   $	��Top=�6c   $	��666��6{      CanUserResizeRows .  	   AutoGenerateColumns	 .  
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ #��6C      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ��  �� Path$	 Num�6N     5      $
 Name��6C    .  6%   �� $
 Name�6Q   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ 	Device ID��6)      CellTemplate 5      �� 5      .R�5      �� -	   5        $��Center=�5   '   $��Center=�6D    �� SelectedItem 6_   �� $ DeviceId���6�   �� $ 	DeviceIds� �� UpdateSourceTrigger$ PropertyChanged=�5      5        5       $ Status��6E    .  6%   �� $ Status�6U     5!      $ Paused��6*    5"      �� 5#      .R�5$      �� -
   5        $��Center=�5$   '   $��Center=�6D    d� 	IsChecked 6�   �� $ IsPaused�$ PropertyChanged=�5%      5'        5(      $ Remove Device��6*    5)      �� 5*      .R�5+      �� -   5        .��5+   B   Remove Device6Q   5,      5.      50      �� 51   
   $��IP Address:��6   $
��Left=�6&   $��27��6A   $��
300,66,0,0q�6M   $	��Top=�6a   $��69��6y   �� 52   
   -   5        $��InstallVJoyButton��52      $��Install vJoy��6*   $
��Left=�6A   $��	57,75,0,0q�6\   $	��Top=�6o   $��75��6�   6�   54      55      