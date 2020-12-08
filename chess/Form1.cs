using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Resources;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private struct cells
        {   
            public string name; 
            public string BorW;// Цвет фигуры
            public int positionX;//Координаты по Х
            public int positionY;// Координаты по У
            public string pos_BorW;//Цвет клетки на которой стоит фигура
        };
        private static cells[,] pole; 
        private static Point position; //координаты выбранной для хода фигуры
        private static bool stat=false; //переменная состояния
        private static int daeth_white = 0, daeth_black = 0;//счетчики срубленных фигур
        private static string player="Белый";// если тру то ходит белый
        private void fill_figurs(bool flag) //расставляем фиогуры на поле
        { 
            if (flag) //черные
            {
                pole[0, 0].name = "Ладья";
                pole[1, 0].name = "Конь";
                pole[2, 0].name = "Слон";
                pole[3, 0].name = "Ферзь";
                pole[4, 0].name = "Король";
                pole[5, 0].name = "Слон";
                pole[6, 0].name = "Конь";
                pole[7, 0].name = "Ладья";
                for (int i = 0; i < 8; i++)
                {
                    pole[i, 1].name = "Пешка";
                    pole[i, 1].BorW = "Черный";
                    pole[i, 0].BorW = "Черный";

                }
            }
            else//белые
            {
                pole[0, 7].name = "Ладья";
                pole[1, 7].name = "Конь";
                pole[2, 7].name = "Слон";
                pole[3, 7].name = "Ферзь";
                pole[4, 7].name = "Король";
                pole[5, 7].name = "Слон";
                pole[6, 7].name = "Конь";
                pole[7, 7].name = "Ладья";
                for (int i = 0; i < 8; i++)
                {
                    pole[i, 6].name = "Пешка";
                    pole[i, 6].BorW = "Белый";
                    pole[i, 7].BorW = "Белый";
                }
            }
        }       
        private void start()//Начальная расстановка
        {
            pole = new cells[8,8];
            fill_figurs(true);
            fill_figurs(false);
            int count = 1; 
            int coordinataX=0,coordinataY=0;
            for (int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {   
                    if (count % 2 > 0)
                        pole[x, y].pos_BorW = "White"; 
                    else
                        pole[x, y].pos_BorW = "Black"; 
                    count++; 
                    pole[x, y].positionX = coordinataX;
                    pole[x, y].positionY = coordinataY;
                    coordinataX += 30;
                }
                count++;
                coordinataY += 30;
                coordinataX = 0;
            }
        }
        private void drawing() 
        {
            Graphics board = this.panel1.CreateGraphics(); 
            Bitmap white_check = new Bitmap(@"figurs\БелыйКлетка.bmp");
            Bitmap black_check = new Bitmap(@"figurs\ЧерныйКлетка.bmp");
            Bitmap temp;
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    if (pole[x, y].name != null) // Если это фигура
                    {
                        temp = new Bitmap(@"figurs\" + pole[x, y].BorW + pole[x, y].name + "На" + pole[x, y].pos_BorW + ".bmp");
                        board.DrawImage(temp, pole[x, y].positionX, pole[x, y].positionY);
                        temp.Dispose();
                    }
                    else
                    {
                        if (pole[x, y].pos_BorW == "White") //Определяем цвет клетки
                            board.DrawImage(white_check, pole[x, y].positionX, pole[x, y].positionY);
                        else
                            board.DrawImage(black_check, pole[x, y].positionX, pole[x, y].positionY); 
                    }
                    white_check.Dispose();
                    black_check.Dispose();
                    board.Dispose();
        }
        private void go(int x, int y) 
        {
            
            if (pole[position.X, position.Y].BorW == player) //проверяем какой игрок сейчас ходит
            {                                                 //и фигуру какого цвета он хочет передвинуть
                if (pole[x, y].name != null) //если игрок хоче кого-то срубить
                {
                    if (pole[x, y].BorW != pole[position.X, position.Y].BorW)
                    {

                        MessageBox.Show("Вы срубили фигуру под названием: " + pole[x, y].name);
                        if (pole[x, y].BorW == "Белый")
                            daeth_white++;
                        else
                            daeth_black++;
                        pole[x, y].name = pole[position.X, position.Y].name;
                        pole[x, y].BorW = pole[position.X, position.Y].BorW;
                        pole[position.X, position.Y].name = null;
                        pole[position.X, position.Y].BorW = null;
                    }
                    else
                    {
                        if (player == "Белый")
                            player = "Черный";
                        else
                            player = "Белый";
                    }
                }
                else
                {
                    pole[x, y].name = pole[position.X, position.Y].name; 
                    pole[x, y].BorW = pole[position.X, position.Y].BorW;
                    pole[position.X, position.Y].name = null;
                    pole[position.X, position.Y].BorW = null;
                }
                if (player == "Белый") 
                    player = "Черный";
                else
                    player = "Белый";
                label1.Text = "Взято белых "+daeth_white.ToString();
                label2.Text = "Взято черных "+daeth_black.ToString();
                label3.Text = "Ходит " + player;
                drawing();
                checkmate();           
            }
            else MessageBox.Show("Сейчас ходит: "+ player);
        }
        private void checkmate()     //пока только есть или нет короля
        {
            bool king_W = true;
            bool king_B = true;
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (pole[i, k].name == "Король" && pole[i, k].BorW == "Белый")
                    {
                        king_W = false;                       
                    }                    
                }                
            }
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (pole[i, k].name == "Король" && pole[i, k].BorW == "Черный")
                    {
                        king_B = false;
                       
                    }                    
                }                
            }
            if (king_W)
            {
                DialogResult result1 = MessageBox.Show("начать заново?", "черные победили",
                MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    start();
                    drawing();
                    daeth_white = 0;
                    daeth_black = 0;
                    player = "Белый";
                    label1.Text = "Взято белых " + daeth_white.ToString();
                    label2.Text = "Взято черных " + daeth_black.ToString();
                    label3.Text = "Ходит " + player;
                }
            }
            if (king_B)
            {
                DialogResult result1 = MessageBox.Show("начать заново?", "белые победили",
                MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    start();
                    drawing();
                    daeth_white = 0;
                    daeth_black = 0;
                    player = "Белый";
                    label1.Text = "Взято белых " + daeth_white.ToString();
                    label2.Text = "Взято черных " + daeth_black.ToString();
                    label3.Text = "Ходит " + player;
                }
            }
        }    
        private void maxmin(ref int max,ref int min,int x,int y,bool x_or_y)//Функция для нахождения минимальной и максимальной координат
        { 
            if(x_or_y) //если тру то сравниваем координаты Х
            {
                if (position.X > x)
                {
                    min = x; //находим минимум и присаваеваем
                    max = position.X;
                }
                else
                {
                    min = position.X+1;
                    max = x;
                }
            }
            else //если false для У
            {
                if (position.Y > y) //тоже самое что и выше, только для У
                {
                    min = y;
                    max = position.Y;
                }
                else
                {
                    min = position.Y+1;
                    max = y;
                }
            }
        }
        private int check_moove(int x, int y) //X Y координаты, куда ходить
        {//Функция проверки корректности ходов всех фигур
            bool flag = true;
            bool bar = false;
            int min=0, max=0;
            int i,k;
            if(pole[position.X, position.Y].name == "Пешка")//проверяем, если выбрана пешка проверяем дальше                     
            {   //проверяем правильность хода для пешки
                if (position.Y == 1 && (position.X == x && position.Y - y == -2) && (pole[x, y].name == null))
                {
                    go(x, y);
                    return 1;
                }

                if (position.Y == 6 && (position.X == x && position.Y - y == 2) && (pole[x, y].name == null))
                {
                    go(x, y);
                    return 1;
                }

                else if ((position.X == x && Math.Abs(position.Y - y) == 1) && pole[x, y].name == null)//тогда ход корректный
                {  
                    if(pole[position.X, position.Y].BorW == "Белый" && position.Y - y != -1)
                        go(x, y);
                    if (pole[position.X, position.Y].BorW == "Черный" && position.Y - y != 1)
                        go(x, y);
                    return 1;
                }
                else if ((Math.Abs(position.X - x)==1) && position.Y - y == 1 && pole[x, y].name != null && pole[position.X,position.Y].BorW == "Белый")
                {
                    go(x, y);
                    return 1;
                }
                else if ((Math.Abs(position.X - x) == 1) && position.Y - y == -1 && pole[x, y].name != null && pole[position.X, position.Y].BorW == "Черный")
                {
                    go(x, y); //передвигаем фигуру, если все условия выполнены
                    return 1;//выходим из функции
                }
            }
            if (pole[position.X, position.Y].name == "Ладья")
            {
                if (position.X != x && position.Y == y)
                {
                    maxmin(ref max,ref min,x,y,true);
                    if (pole[x, y].name != null)
                    {
                        bar = true;
                        min++;
                    }
                    for (i = min; i < max; i++)
                        if (pole[i, y].name != null)
                        {
                            flag = false;
                            break;
                        }
                        if ((flag && bar) || flag)
                        {
                            go(x, y);
                            return 1;
                        }
                }
                if (position.X == x && position.Y != y)
                {
                    maxmin(ref max,ref min,x,y,false);
                    if (pole[x, y].name != null)
                    {
                        bar = true;
                        min++;
                    }
                    for (i = min; i < max; i++) 
                        if (pole[x, i].name != null )
                        {
                            flag = false;
                            break;
                        }
                    if ((flag&&bar) ||flag)
                    { 
                        go(x, y);
                        return 1;
                    }
                }
            } 
            if (pole[position.X, position.Y].name == "Конь")
            {
                if ((Math.Abs(position.X - x) == 2 && Math.Abs(position.Y - y) == 1) || (Math.Abs(position.X - x) == 1 && Math.Abs(position.Y - y) == 2))
                {
                    go(x, y);
                    return 1;
                }
            }
            if (pole[position.X, position.Y].name == "Слон")
            {
                if (position.X > x && position.Y > y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X-1, k = position.Y-1; x < i && y < k ; i--, k--)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X < x && position.Y < y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X + 1, k = position.Y + 1; i < x && k < y; i++, k++)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X > x && position.Y < y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X - 1, k = position.Y + 1; x < i && k < y; i--, k++)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X < x && position.Y > y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X + 1, k = position.Y - 1; i < x && y < k; i++, k--)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
            }
            if (pole[position.X, position.Y].name == "Король")
            {
                if (Math.Abs(position.X - x) <= 1 && Math.Abs(position.Y - y) <= 1)
                {
                    go(x,y);
                    drawing();
                    return 1;
                }
            }
            if (pole[position.X, position.Y].name == "Ферзь")
            {
                if (position.X > x && position.Y > y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X - 1, k = position.Y - 1; x < i && y < k; i--, k--)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X < x && position.Y < y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X + 1, k = position.Y + 1; i < x && k < y; i++, k++)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X > x && position.Y < y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X - 1, k = position.Y + 1; x < i && k < y; i--, k++)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X < x && position.Y > y)
                {
                    if (Math.Abs(position.X - x) == Math.Abs(position.Y - y))
                    {
                        for (i = position.X + 1, k = position.Y - 1; i < x && y < k; i++, k--)
                            if (pole[i, k].name != null)
                            {
                                flag = false;
                                break;
                            }
                        if (flag)
                        {
                            go(x, y);
                            return 1;
                        }
                    }
                }
                if (position.X != x && position.Y == y)
                {
                    maxmin(ref max, ref min, x, y, true);
                    if (pole[x, y].name != null)
                    {
                        bar = true;
                        min++;
                    }
                    for (i = min; i < max; i++)
                        if (pole[i, y].name != null)
                        {
                            flag = false;
                            break;
                        }
                    if ((flag && bar) || flag)
                    {
                        go(x, y);
                        return 1;
                    }
                }
                if (position.X == x && position.Y != y)
                {
                    maxmin(ref max, ref min, x, y, false);
                    if (pole[x, y].name != null)
                    {
                        bar = true;
                        min++;
                    }
                    for (i = min; i < max; i++)
                        if (pole[x, i].name != null)
                        {
                            flag = false;
                            break;
                        }
                    if ((flag && bar) || flag)
                    {
                        go(x, y);
                        return 1;
                    }
                }            
            }       
            stat = false;
            drawing();
            return 0;
        }
        private int check_Koordinate(int c) //в эту функцию передаются координаты клеток в пикселях
        {              
            if (c < 30) return 0;
            if (c > 30 && c < 60) return 1;
            if (c > 60 && c < 90) return 2;
            if (c > 90 && c < 120) return 3;
            if (c > 120 && c < 150) return 4;
            if (c > 150 && c < 180) return 5;
            if (c > 180 && c < 210) return 6;
            if (c > 210 && c < 240) return 7;
            return -1;
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (check_Koordinate(e.X) >= 0 && check_Koordinate(e.Y) >= 0)//проверяем не выходит ли если координата за границы
                if (stat)
                {
                    check_moove(check_Koordinate(e.X), check_Koordinate(e.Y));
                    stat = false;
                }
                else
                {
                    if (pole[check_Koordinate(e.X), check_Koordinate(e.Y)].name == null)
                    stat = false;
                    else
                    {
                        position.X = check_Koordinate(e.X);
                        position.Y = check_Koordinate(e.Y);
                        stat = true;
                    }
                }    
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            drawing();
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            drawing();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            start();
            drawing();
            player = "Белый";
            label1.Text = "Взято белых " + daeth_white.ToString();
            label2.Text = "Взято черных " + daeth_black.ToString();                  
            label3.Text = "Ходит " + player;            
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            drawing();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawing();
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void играToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void начатьНовуюИгруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start();
            drawing();
            daeth_white = 0;
            daeth_black = 0;
            player = "Белый";
            label1.Text = "Взято белых " + daeth_white.ToString();
            label2.Text = "Взято черных " + daeth_black.ToString();
            label3.Text = "Ходдит " + player;
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}