   M S B A M L   `   `   `  ���� L  HAndroidController, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AndroidControllerAndroidController  "    AndroidController.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:AndroidController     -     � Title$  AndroidControllerServer��$	��450��$	��800��.��� .+��� $��Android Controller Server��$
��Left=�$��36��$��317,21,0,364q�$��Center=�$	��168���� -    ��Name$ ServerStatusButton��$��Start Server��$
��Left=�$��23��$��
57,104,0,0q�$	��Top=�$��81���� -    ��Name$ ServerStatusLabel��$��Server Inactive��$
��Left=�$��23��$��138,104,0,0q�$	��Top=�$��89���� $��Available Virtual Devices��$
��Left=�$��
583,21,0,0q�$	��Top=�$��27��$	��140���� -    ��Name$ DevicesListBox��$
��Left=�$��52��$��
583,48,0,0q�$	��Top=�$	��140���� -   $ VJoyStatusLabel��$��vJoy Disabled��$
��Left=�$��	57,44,0,0q�$	��Top=�$	��170���� -    ��Name$ IpAddressTextBox��$
��Left=�$��20��$��
374,73,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��120�� �� IsReadOnlyCaretVisible . �� $��Port:��$
��Left=�$��28��$��
300,98,0,0q�$	��Top=�$��69���� -   $ PortTextBox��$
��Left=�$��23��$
 Wrap=�$�� ��$	��Top=�$	��120��$��374,104,0,0q��� $��	Devices
��$
��Left=�$��
57,135,0,0q�$	��Top=�$��25��&   System.Windows.Controls.DataGrid  -     Name$ 
SocketGrid��$
��Left=�$	��218��$��
57,160,0,0q�$	��Top=�$	��666��   CanUserResizeRows .  	   AutoGenerateColumns	 .  
   Columns
 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ #��   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$	 Num�  $
 Name�� .  �� $
 Name�4  .System.Windows.Controls.DataGridTemplateColumn  $ 	Device ID��   CellTemplate �� .R��� -	   $��Center=�$��Center=� �� SelectedItem �� $ DeviceId����� $ 	DeviceIds� �� UpdateSourceTrigger$ PropertyChanged=�  $ Status�� .  �� $ Status�  $ Paused�� �� .R��� -
   $��Center=�$��Center=� d� 	IsChecked �� $ IsPaused�$ PropertyChanged=�  $ Remove Device�� �� .R��� -   .��Remove Device�� $��IP Address:��$
��Left=�$��27��$��
300,66,0,0q�$	��Top=�$��69��