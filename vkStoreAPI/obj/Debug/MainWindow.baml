   M S B A M L   `   `   `  ����E  AvkStoreAPI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:vkStoreAPI
vkStoreAPI  5   	       vkStoreAPI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:vkStoreAPI     5   	   -   5          � Title$  vkGroupGoods��5   	   $	��700��6   $
��1200��6+    � 
ResizeMode$ NoResize=�6Q    � WindowState$ 	Maximized=�6g    � WindowStartupLocation$ CenterScreen=�6   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��184* �6   �� 5      $	��55* �6   �� 5      $
��369* �6   �� 5      $	��64* �6   .+�5   
   �� -   5         ��Name$ horSp1��5       �� Orientation$ 
Horizontal=�6%   
�������$��0��6V   .+�5      �� -   5         ��Name$ 
mainHeader��5      $!��Мои сообщества��6(   $
��Left=�6A   $��400,80,0,79q�6\   $	��Top=�6q   $��32(�6�   $��52��6�   $
��Auto��6�   5      �� 5   
   -   5        $ spGroups��5      $��2��6'   $��0,11,0,0q�64   .+�5      �� -   5        $ titleGroups��5      $��20(�6)   .��67   ?=Выберите одно из своих сообществ:6Z   �� 5      -   5         ��Name$ lbGroupd��5      $��0q�6(   6k   5      �� 5   
   -   5        $ spGroupsProducts��5      $��1��6/   $y�	Collapsed=�6<   $ 
Horizontal=�6S   	 �� CanVerticallyScroll	 . $��2��6�   .+�5      �� -   5        $ lbGroupProducts��5      $��1��6/   $	��400��6<   $
��1000��6I   $
��Left=�6V   $��0q�6q   6�   �� 5      -	   5        $ 	spButtons��5      $��1��6,   .+�5      �� -
   5        
 ��Name$
 btnAdd��5      $��50, 50, 0, 0q�6)   $	��200��6?   $��40��6K   .��6l   Добавить товар6|   �� 5      -   5        $
 btnDel��5      $��50, 50, 0, 0q�6)   $	��200��6?   $��40��6K   .��6l   Удалить товар6{   �� 5      -   5        $
 btnAlter��5      $��50, 50, 0, 0q�6+   $	��200��6A   $��40��6M   .��6q   Изменить товар6�   5      5      �� 5   
   -   5        $
 btnBack��5      $��3��6"   $	��100��6/   $��
50,22,0,22q�6;   $
��Left=�6O   $y�Visible=�6j   {�.  .��6�   
Назад6�   5       5!      