using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

Form scherm = new Form();
scherm.Text = "MandelBrot";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(420, 580);

// Create all GUI elements
Button goBtn = new Button();
goBtn.Text = "GO!";
goBtn.BackColor = Color.White;
// Labels
Label middenXLabel = new Label();
middenXLabel.Text = "MiddenX";
Label middenYLabel = new Label();
middenYLabel.Text = "MiddenY";
Label schaalLabel = new Label();
schaalLabel.Text = "schaal";
Label maxAantalLabel = new Label();
maxAantalLabel.Text = "maxAantal";

Label debug = new Label();
debug.Location = new Point(400, 10);
debug.Size = new Size(30, 30);
scherm.Controls.Add(debug);

// InputBoxes
TextBox middenXInput = new TextBox();
TextBox middenYInput = new TextBox();
TextBox schaalInput = new TextBox();
TextBox maxAantalInput = new TextBox();

//Other
ProgressBar progressBar = new ProgressBar();

// Specify location of the elements
goBtn.Location = new Point(250, 100);

middenXLabel.Location = new Point(10, 10);
middenYLabel.Location = new Point(10, 40);
schaalLabel.Location = new Point(10, 70);
maxAantalLabel.Location = new Point(10, 100);

middenXInput.Location = new Point(120, 10);
middenYInput.Location = new Point(120, 40);
schaalInput.Location = new Point(120, 70);
maxAantalInput.Location = new Point(120, 100);

progressBar.Location = new Point(10, 550);

// Specify Size of the Elements
goBtn.Size = new Size(50, 25);
middenXLabel.Size = new Size(100, 30);
middenYLabel.Size = new Size(100, 30);
schaalLabel.Size = new Size(100, 30);
maxAantalLabel.Size = new Size(100, 30);

middenXInput.Size = new Size(200, 30);
middenYInput.Size = new Size(200, 30);
schaalInput.Size = new Size(200, 30);
maxAantalInput.Size = new Size(100, 30);

progressBar.Size = new Size(400, 20);
progressBar.BackColor = Color.DarkGray;

middenXInput.Text = "0";
middenYInput.Text = "0";
schaalInput.Text = "1";
maxAantalInput.Text = "100";

// Add all elements to the screen
scherm.Controls.Add(goBtn);

scherm.Controls.Add(middenXLabel);
scherm.Controls.Add(middenYLabel);
scherm.Controls.Add(schaalLabel);
scherm.Controls.Add(maxAantalLabel);

scherm.Controls.Add(middenXInput);
scherm.Controls.Add(middenYInput);
scherm.Controls.Add(schaalInput);
scherm.Controls.Add(maxAantalInput);

scherm.Controls.Add(progressBar);

// Create Image Box
Bitmap ImageBoxDrawing = new Bitmap(400, 400);
Graphics ImageBoxDrawer = Graphics.FromImage(ImageBoxDrawing);

//create Label
Label ImageBoxImage = new Label();
scherm.Controls.Add(ImageBoxImage);
ImageBoxImage.Location = new Point(10, 140);

//Image Size Variable
double imageSize = 400;
ImageBoxImage.Size = new Size(400, 400);

ImageBoxImage.BackColor = Color.White;

ImageBoxImage.Image = ImageBoxDrawing;

//button event
goBtn.Click += GoBtn_Click;

void GoBtn_Click(object sender, EventArgs e)
{
    IterateTroughPixels();
}

void IterateTroughPixels()
{
    if (ValidateInputs())
    {
        float x = float.Parse(middenXInput.Text);
        float y = float.Parse(middenYInput.Text);
        float schaal = float.Parse(schaalInput.Text);

        ImageBoxImage.Invalidate();
        //loop trough every pixel width and make it between a range between -2 and 2
        for (float i = 0; i <= imageSize; i++) {
            //Rows
            progressBar.Value = Convert.ToInt32((i / imageSize) * 100);
            double remappedPixelX = (-2 + (x / imageSize) + (i / (imageSize / 4))) * schaal;
            for (float j = 0; j <= 400; j++)
            {
                //Columns
                double remappedPixelY = (-2 + (y / imageSize) + (j / (imageSize / 4))) * schaal;
                Brush pixelColor = Brushes.White;
                if (calculateMandelBrotFromPoint(remappedPixelX, remappedPixelY) % 2 == 0)
                {
                    pixelColor = Brushes.Black;
                }
                ImageBoxDrawer.FillRectangle(pixelColor, i, j, 1, 1);
            }

        }
    }
    else
    {
        goBtn.Text = "Invalid Inputs";
    }
}

bool ValidateInputs() {
    //Validate if all inputs from user are valid for calculation
    return true;
}

int calculateMandelBrotFromPoint(double x, double y){

    int teller = 0;
    //variables
    //mandelBrot A & B start at 0
    double mandelBrotA = 0;
    double mandelBrotB = 0;
    //Give a maximum to the calculation
    
    for (int i = 0; i < Int32.Parse(maxAantalInput.Text); i++)
    {
        if (CheckPythagoras(mandelBrotA, mandelBrotB, 0, 0))
        {
            double oldMandelBrotA = mandelBrotA;
            //CalculateMandelGetalX
            mandelBrotA = Math.Pow(mandelBrotA, 2) - Math.Pow(mandelBrotB, 2) + x;
            //CalculateMandelGetalY
            mandelBrotB = 2 * oldMandelBrotA * mandelBrotB + y;
            teller++;
        }
        else
        {
            break;
        }
    }
    return teller;

}


 bool CheckPythagoras(double ax, double ay, double bx, double by)
{
    // Check if distance between 2 points < 2, return bool
    double sqrt = Math.Pow((ax-bx), 2) + Math.Pow((ay-by), 2);   
    return true ? sqrt < 4 : false;
}


ImageBoxImage.MouseDoubleClick += ImageBoxImage_MouseDoubleClick;

void ImageBoxImage_MouseDoubleClick(object sender, MouseEventArgs e)
{
    middenXInput.Text = (Int32.Parse(middenXInput.Text) +  (e.X - 200)*4).ToString();
    middenYInput.Text = (Int32.Parse(middenYInput.Text) + (e.Y - 200)*4).ToString();
    //schaalInput.Text = (Int32.Parse(schaalInput.Text) * 0.1).ToString();
    IterateTroughPixels();
}

Application.Run(scherm);