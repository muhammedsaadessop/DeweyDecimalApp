   M S B A M L   `   `   `  ����L  Hdewey decimal app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:dewey_decimal_appdewey_decimal_app  5   	   '    !dewey_decimal_app.bookidentyfying   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:dewey_decimal_app     5   	     � Title$  bookidentyfying��5   	   $	��500��6!   $
��1300��6.    � 
ResizeMode$ CanMinimize=�5	      .��5
      � $��-6,0,6,0q�6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      ��5   
   �� 5       �� ImageSource$ /bc.jpg��6   .+�5   
   �� $��MATCH THE BOOKS��6   $��Center=�6*   $��0,10,0,0q�6G   $	��Top=�6Y   $��20(�6q   $��Arial Rounded MT Bold)�6   $
��Bold"�6�   ���	#FFA29A9A$��0��6�   $��2��6�   $��1��6�   ��5      �� 5      5      �� 5   
   -   5         ��Name$ Booknumbers��5      $��1��6'   $��1��6�   ��5      �� 5      .R�5       �� ��6    �� 5!      5#      �� 5%   
   -   5        $ booknamelist��5%      $��2��6(   $��1��6�   ��5&      �� 5'      .R�5(      �� ��6    �� 5)      5+      &   System.Windows.Controls.DataGrid  5-   
   -   5          Name$ display_output��5-      $��3��6+      AutoGenerateColumns .   J� 	AllowDrop . $��1��6}   	   Columns	 5.      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Book number��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *	BookNum+  50      $
 Description��6%    6:   �� *BookNm+  51      $
 Score��6%    64   �� *Score+��53      �� 54      6   56      �� 57   
   -   5         ��Name$ Display��57      $��Change List��6"   $
��Left=�68   $��10,0,0,0q�6S   $	��Top=�6e   ���	#FFB9F58C$��35��6�   $	��310��6�   ���	#FF464646$��1��6�   ��58      �� 59      5;      �� 5=   
   -   5        $��
Backbutton��5=      $��	0,0,198,5q�69   $��0q�6L   $��0��6`   $��0��6m   ��5>      �� 5?      6   .��5A      �� $��/return-88.png��6   $��Uniform=�6,   $	��148��6>   $��45��6J   5B      �� 5D   
   -   5        $ bronze_badge��5D      $��1��6B   $��0��6O   $��10,42,218,299q�6_    J� RenderTransformOrigin$ 0.707,0.763&�6v   ��5F      �� 5G      $ /Bronze_Icon.png��6   6>   5J      �� 5K   
   -   5        $ silver_badge��5K      $��1��6C   $��115,47,93,304q�6P   $ 0.707,0.763&�6g   z�5L      V� 5M      .~�5N      �� �� 5O      �� 5P       �� Angle$ -0.032Q�6&   T� 5Q      5R      ��5T      �� 5U      $# /Silver-Medal-PNG-Clipart.png��6    j� Stretch$ Uniform=�6I    j� TileMode$
 None=�6[   6m   5W      �� 5X   
   -   5        $ 
gold_badge��5X      $��10,155,218,205q�6>   $��1��6V   ��5Y      �� 5Z      $ /Award-PNG.png��6   6<   5\      5`      5a      