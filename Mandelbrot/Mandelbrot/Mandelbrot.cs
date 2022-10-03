using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

Form scherm = new Form();
scherm.Text = "MandelBrot";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(420, 610);

// Create all GUI elements
Button goBtn = new Button();
goBtn.Text = "GO!";
goBtn.BackColor = Color.White;

Button undoBtn = new Button();
undoBtn.Text = "Undo";
undoBtn.BackColor = Color.White;
// Labels
Label middenXLabel = new Label();
middenXLabel.Text = "MiddenX";
Label middenYLabel = new Label();
middenYLabel.Text = "MiddenY";
Label schaalLabel = new Label();
schaalLabel.Text = "schaal";
Label maxAantalLabel = new Label();
maxAantalLabel.Text = "maxAantal";
Label dropdownLabel = new Label();
dropdownLabel.Text = "Optional preset:";

// InputBoxes
TextBox middenXInput = new TextBox();
TextBox middenYInput = new TextBox();
TextBox schaalInput = new TextBox();
TextBox maxAantalInput = new TextBox();
ComboBox dropdown = new ComboBox();

//Other
ProgressBar progressBar = new ProgressBar();

// Specify location of the elements
goBtn.Location = new Point(250, 100);
undoBtn.Location = new Point(310, 100);

middenXLabel.Location = new Point(10, 10);
middenYLabel.Location = new Point(10, 40);
schaalLabel.Location = new Point(10, 70);
maxAantalLabel.Location = new Point(10, 100);
dropdownLabel.Location = new Point(10, 135);

middenXInput.Location = new Point(120, 10);
middenYInput.Location = new Point(120, 40);
schaalInput.Location = new Point(120, 70);
maxAantalInput.Location = new Point(120, 100);
dropdown.Location = new Point(120, 135);

progressBar.Location = new Point(10, 580);

// Specify Size of the Elements
goBtn.Size = new Size(50, 25);
undoBtn.Size = new Size(50, 25);

middenXLabel.Size = new Size(100, 30);
middenYLabel.Size = new Size(100, 30);
schaalLabel.Size = new Size(100, 30);
maxAantalLabel.Size = new Size(100, 30);
dropdownLabel.Size = new Size(100, 30);

middenXInput.Size = new Size(200, 30);
middenYInput.Size = new Size(200, 30);
schaalInput.Size = new Size(200, 30);
maxAantalInput.Size = new Size(100, 30);
dropdown.Size = new Size(200, 20);

progressBar.Size = new Size(400, 20);
progressBar.BackColor = Color.DarkGray;

middenXInput.Text = "0";
middenYInput.Text = "0";
schaalInput.Text = "1";
maxAantalInput.Text = "100";

string[] presets = { "Hugooo", "KOEN" };
string[] middenXPresets = { "100", "-25,4903564453125" };
string[] middenYPresets = { "100", "-65,5469970703125" };
string[] schaalPresets = { "0,25", "6,103515625E-05" };

dropdown.Items.AddRange(presets);

// Add all elements to the screen
scherm.Controls.Add(goBtn);
scherm.Controls.Add(undoBtn);

scherm.Controls.Add(middenXLabel);
scherm.Controls.Add(middenYLabel);
scherm.Controls.Add(schaalLabel);
scherm.Controls.Add(maxAantalLabel);
scherm.Controls.Add(dropdownLabel);

scherm.Controls.Add(middenXInput);
scherm.Controls.Add(middenYInput);
scherm.Controls.Add(schaalInput);
scherm.Controls.Add(maxAantalInput);
scherm.Controls.Add(dropdown);

scherm.Controls.Add(progressBar);

// Create Image Box
Bitmap ImageBoxDrawing = new Bitmap(400, 400);
Graphics ImageBoxDrawer = Graphics.FromImage(ImageBoxDrawing);

//create Label
Label ImageBoxImage = new Label();
scherm.Controls.Add(ImageBoxImage);
ImageBoxImage.Location = new Point(10, 170);

//Image Size Variable
double imageSize = 400;
ImageBoxImage.Size = new Size(400, 400);

ImageBoxImage.BackColor = Color.White;

ImageBoxImage.Image = ImageBoxDrawing;

//variables
int maxAantal = 100;

string[] savedMiddenXInput = { "0" };
string[] savedMiddenYInput = { "0" };
string[] savedSchaalInput = { "1" };
string[] savedMaxAantalInput = { "100" };
int currentUndoPosition = 0;

//button event
goBtn.Click += GoBtn_Click;
void GoBtn_Click(object sender, EventArgs e)
{
    currentUndoPosition = 0;
    IterateTroughPixels();
}

void IterateTroughPixels()
{
    if (ValidateInputs(middenXInput.Text, middenYInput.Text, schaalInput.Text, maxAantalInput.Text))
    {
        maxAantal = Int32.Parse(maxAantalInput.Text);

        if (currentUndoPosition == 0)
        {
            Array.Resize(ref savedMiddenXInput, savedMiddenXInput.Length + 1);
            savedMiddenXInput[savedMiddenXInput.GetUpperBound(0)] = middenXInput.Text;
            Array.Resize(ref savedMiddenYInput, savedMiddenYInput.Length + 1);
            savedMiddenYInput[savedMiddenYInput.GetUpperBound(0)] = middenYInput.Text;
            Array.Resize(ref savedSchaalInput, savedSchaalInput.Length + 1);
            savedSchaalInput[savedSchaalInput.GetUpperBound(0)] = schaalInput.Text;
            Array.Resize(ref savedMaxAantalInput, savedMaxAantalInput.Length + 1);
            savedMaxAantalInput[savedMaxAantalInput.GetUpperBound(0)] = maxAantalInput.Text;
        }

        float x = float.Parse(middenXInput.Text);
        float y = float.Parse(middenYInput.Text);
        float schaal = float.Parse(schaalInput.Text);

        ImageBoxImage.Invalidate();
        //loop trough every pixel width and make it between a range between -2 and 2
        for (float i = 0; i <= imageSize; i++) {
            //Rows
            progressBar.Value = Convert.ToInt32((i / imageSize) * 100);
            //imageSize / 4 --> 400/4 = 100 --> i/100 gives range between -2 and 2
            //* schaal --> scretches image in both x and y to make it appear it is zoomed in
            double remappedPixelX = (-2 + (i / (imageSize/4)) + ((x/schaal) / (imageSize / 4))) * schaal;
            for (float j = 0; j <= 400; j++)
            {
                //Columns
                double remappedPixelY = (-2 + ((y /schaal) / (imageSize/4)) + (j / (imageSize / 4))) * schaal;
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

bool ValidateInputs(string middenX, string middenY, string schaal, string mAantal) {
    //Validate if all inputs from user are valid for calculation
    if (middenX == "" || middenY == "" || schaal == "" || mAantal == "") {
        return false;
    }
    return true;
}



int calculateMandelBrotFromPoint(double x, double y){

    int teller = 0;
    //variables
    //mandelBrot A & B start at 0
    double mandelBrotA = 0;
    double mandelBrotB = 0;
    //Give a maximum to the calculation
    
    for (int i = 0; i < maxAantal; i++)
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
    schaalInput.Text = (double.Parse(schaalInput.Text) / 2).ToString();
    middenXInput.Text = ((double.Parse(middenXInput.Text) + (e.X - 200) * double.Parse(schaalInput.Text))).ToString();
    middenYInput.Text = ((double.Parse(middenYInput.Text) + (e.Y - 200) * double.Parse(schaalInput.Text))).ToString();

    currentUndoPosition = 0;
    IterateTroughPixels();
}

//features

//enter on inputs
maxAantalInput.KeyDown += Input_KeyDown;
middenXInput.KeyDown += Input_KeyDown;
middenYInput.KeyDown += Input_KeyDown;
schaalInput.KeyDown += Input_KeyDown;

void Input_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.Enter) {
        IterateTroughPixels();
    }
};

//Undo button
undoBtn.Click += UndoBtn_Click;

void UndoBtn_Click(object sender, EventArgs e)
{
    currentUndoPosition += 1;
    if (currentUndoPosition < savedMiddenXInput.Length && ValidateInputs(savedMiddenXInput[savedMiddenXInput.Length - 1 - currentUndoPosition],
        savedMiddenYInput[savedMiddenYInput.Length - 1 - currentUndoPosition],
        savedSchaalInput[savedSchaalInput.Length - 1 - currentUndoPosition],
        savedMaxAantalInput[savedMaxAantalInput.Length - 1 - currentUndoPosition]))
    {

        middenXInput.Text = savedMiddenXInput[savedMiddenXInput.Length - 1 - currentUndoPosition];
        middenYInput.Text = savedMiddenYInput[savedMiddenYInput.Length - 1 - currentUndoPosition];
        schaalInput.Text = savedSchaalInput[savedSchaalInput.Length - 1 - currentUndoPosition];
        maxAantalInput.Text = savedMaxAantalInput[savedMaxAantalInput.Length - 1 - currentUndoPosition];
        IterateTroughPixels();
    }
    
};

dropdown.SelectedIndexChanged += Dropdown_SelectedIndexChanged;

void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
{
    middenXInput.Text = middenXPresets[dropdown.SelectedIndex];
    middenYInput.Text = middenYPresets[dropdown.SelectedIndex];
    schaalInput.Text = schaalPresets[dropdown.SelectedIndex];
    currentUndoPosition = 0;
    IterateTroughPixels();
}

Application.Run(scherm);