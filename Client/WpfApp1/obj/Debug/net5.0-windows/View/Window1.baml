   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WpfApp1.ViewModelWpfApp1.ViewModel  5   	   +clr-namespace:WpfApp1.ViewWpfApp1.View  5   	       WpfApp1.View.Window1   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .vmclr-namespace:WpfApp1.ViewModel     5   	   ,localclr-namespace:WpfApp1.View     5   	   $	��600��5	   &   $
��1025��63     � Title  6	   �� *
namePlayer+.��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��3* �6   �� 5      $��1* �6   .+�5   
   �� $��2��6   .+�5      �� -   5         ��Name$ listMessageServText��5      $��Message du serveur: ��65   �� 5      -   5         ��Name$ listMessageServ��5      ��6/   �� *MessageServ+��5      �� 5      .R�5      �� $��5q�6%   .+�5       �� ��6(   �� 5!      5"      5$      �� 5&      -   5        $
 text��5&      ��6&   �� *Text+�� 5(      -   5        $ listHand��5(       �� SelectionMode$ Single=�6c   ��6(   �� *Hand+��5*      �� 5+      .R�5,      ��  �� Orientation$ 
Horizontal=�6%   5-      ��5/      �� 51      .R�53      �� $��5q�6%   .+�54      �� ��6(   �� *Num+�� 55      $+��%/WpfApp1;component/images/taureau.png��6$   $��30��6T   $��30��6`   �� 56      
��� �����6(   �� *Malus+6?   59      5:      5<      �� 5=      $��Envoyer au Serveur��6U   ��6   �� *ClickCommand+{�66   �� *
CanClose+6U   �� 5>      ��6   �� *
TextServer+5?      �� 5@   
   -   5        $ listBox1��5@   6   $��2��6   $��2��6"   $ Single=�6�    �� AlternationCount$	 100��6�   ��6H   �� *Board+��5B      �� 5C      .R�5D      �� .+�5E      �� ��6$   �� *Index+ �� StringFormat$ 
Ligne: {0}���� 5F      $ listBox2��6"   ��64   �� *Line+��5I      �� 5J   "   .R�5K   &   �� $ 
Horizontal=�61   5L   #   ��5N      �� 5P   "   .R�5R   &   �� $��5q�61   .+�5S   *   �� ��64   �� *Num+�� 5T   *   $+��%/WpfApp1;component/images/taureau.png��60   $��30��6`   $��30��6l   �� 5U   *   
��� �����64   �� *Malus+6K   5W   '   5X   #   5Z      5[      5\      5^      �� 5`   
   $��2��6   $��1��6$   $��Right=�61   $
��Bold"�6N   $��12(�6`   
���  ��$��Malus:��6�   �� 5a   
   $��3��6   $��1��6$   ��61   �� *Score+5b      5c      