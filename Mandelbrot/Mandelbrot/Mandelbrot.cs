using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

Form scherm = new Form();
scherm.Text = "MandelBrot";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(600, 610);

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
schaalLabel.Text = "Schaal";
Label maxAantalLabel = new Label();
maxAantalLabel.Text = "MaxAantal";
Label dropdownLabel = new Label();
dropdownLabel.Text = "Preset";
Label kleurLabel = new Label();
kleurLabel.Text = "Kleur";

// InputBoxes
TextBox middenXInput = new TextBox();
TextBox middenYInput = new TextBox();
TextBox schaalInput = new TextBox();
TextBox maxAantalInput = new TextBox();
ComboBox dropdown = new ComboBox();
ComboBox kleurDropDown = new ComboBox();

// zet de variabelen
int[] kleur = {128, 128, 128};
int xOffset = 20;

//Other
ProgressBar progressBar = new ProgressBar();
Panel inputsPanel = new Panel();
Panel extrasPanel = new Panel();

// Specify location of the elements
goBtn.Location = new Point(250, 100);
undoBtn.Location = new Point(310, 100);

middenXLabel.Location = new Point(xOffset, 10);
middenYLabel.Location = new Point(xOffset, 40);
schaalLabel.Location = new Point(xOffset, 70);
maxAantalLabel.Location = new Point(xOffset, 100);
dropdownLabel.Location = new Point(xOffset, 135);
kleurLabel.Location = new Point(330 + xOffset, 10);

middenXInput.Location = new Point(110 + xOffset, 10);
middenYInput.Location = new Point(110 + xOffset, 40);
schaalInput.Location = new Point(110 + xOffset, 70);
maxAantalInput.Location = new Point(110 + xOffset, 100);
dropdown.Location = new Point(110 + xOffset, 135);
kleurDropDown.Location = new Point(390 + xOffset, 10);

progressBar.Location = new Point(100, 580);
inputsPanel.Location = new Point(10, 10);
extrasPanel.Location = new Point(200, 10);

// Specify Size of the Elements
goBtn.Size = new Size(50, 25);
undoBtn.Size = new Size(50, 25);

middenXLabel.Size = new Size(100, 25);
middenYLabel.Size = new Size(100, 25);
schaalLabel.Size = new Size(100, 25);
maxAantalLabel.Size = new Size(100, 25);
dropdownLabel.Size = new Size(100, 25);
kleurLabel.Size = new Size(50, 25);

middenXInput.Size = new Size(200, 30);
middenYInput.Size = new Size(200, 30);
schaalInput.Size = new Size(200, 30);
maxAantalInput.Size = new Size(100, 30);
dropdown.Size = new Size(200, 20);
kleurDropDown.Size = new Size(180, 20);

progressBar.Size = new Size(400, 20);
progressBar.BackColor = Color.DarkGray;
inputsPanel.Size = new Size(400, 180);
extrasPanel.Size = new Size(400, 180);

middenXInput.Text = "0";
middenYInput.Text = "0";
schaalInput.Text = "1";
maxAantalInput.Text = "100";

string[] presetsKleur = {"roze", "groen", "rood", "wit", "kleuren"};
string[] presets = { "Spiraal", "Mandelbrot in mandelbrot" };
string[] middenXPresets = { "36.98388671875", "-59.815625" };
string[] middenYPresets = { "-67.1396484375", "-66.23906249999999" };
string[] schaalPresets = { "0.000244140625", "0.00234375" };

kleurDropDown.Items.AddRange(presetsKleur);
dropdown.Items.AddRange(presets);


// Add all elements to the screen
scherm.Controls.Add(goBtn);
scherm.Controls.Add(undoBtn);

scherm.Controls.Add(middenXLabel);
scherm.Controls.Add(middenYLabel);
scherm.Controls.Add(schaalLabel);
scherm.Controls.Add(maxAantalLabel);
scherm.Controls.Add(dropdownLabel);
scherm.Controls.Add(kleurLabel);

scherm.Controls.Add(middenXInput);
scherm.Controls.Add(middenYInput);
scherm.Controls.Add(schaalInput);
scherm.Controls.Add(maxAantalInput);
scherm.Controls.Add(dropdown);
scherm.Controls.Add(kleurDropDown);

scherm.Controls.Add(progressBar);
scherm.Controls.Add(inputsPanel);
scherm.Controls.Add(extrasPanel);

// Create Image Box
Bitmap ImageBoxDrawing = new Bitmap(400, 400);
Graphics ImageBoxDrawer = Graphics.FromImage(ImageBoxDrawing);

// create Label
Label ImageBoxImage = new Label();
scherm.Controls.Add(ImageBoxImage);
ImageBoxImage.Location = new Point(100, 170);

// Image Size Variable
double imageSize = 400;
ImageBoxImage.Size = new Size(400, 400);

ImageBoxImage.BackColor = Color.White;

ImageBoxImage.Image = ImageBoxDrawing;

// UI improvements
// scherm.BackColor = Color.Black;
// scherm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
// inputsPanel.BackColor = Color.DimGray;
// extrasPanel.BackColor = Color.DimGray;

Font font = new Font("Montserrat", 10, FontStyle.Bold);
middenXLabel.Font = font;
middenYLabel.Font = font;
schaalLabel.Font = font;
maxAantalLabel.Font = font;
dropdownLabel.Font = font;
kleurLabel.Font = font;
middenXLabel.BackColor = Color.Transparent;
middenYLabel.BackColor = Color.Transparent;
schaalLabel.BackColor = Color.Transparent;
maxAantalLabel.BackColor = Color.Transparent;
dropdownLabel.BackColor = Color.Transparent;
kleurLabel.BackColor = Color.Transparent;
middenXLabel.TextAlign = ContentAlignment.MiddleLeft;
middenYLabel.TextAlign = ContentAlignment.MiddleLeft;
schaalLabel.TextAlign = ContentAlignment.MiddleLeft;
maxAantalLabel.TextAlign = ContentAlignment.MiddleLeft;
dropdownLabel.TextAlign = ContentAlignment.MiddleLeft;
kleurLabel.TextAlign = ContentAlignment.MiddleLeft;



// variables
int maxAantal = 100;

string[] savedMiddenXInput = { "0" };
string[] savedMiddenYInput = { "0" };
string[] savedSchaalInput = { "1" };
string[] savedMaxAantalInput = { "100" };
int currentUndoPosition = 0;
bool kleurZelfGekozen = false;

// button event
goBtn.Click += GoBtn_Click;
void GoBtn_Click(object sender, EventArgs e)
{
    currentUndoPosition = 0;
    IterateTroughPixels(kleurZelfGekozen);
}

void IterateTroughPixels(bool kleurZelfGekozen=false)
{
    if (ValidateInputs(middenXInput.Text, middenYInput.Text, schaalInput.Text, maxAantalInput.Text))
    {
        maxAantal = Int32.Parse(maxAantalInput.Text);


        // If not undoing
        if (currentUndoPosition == 0)
        {
            // push all values to arrays for later undoing
            Array.Resize(ref savedMiddenXInput, savedMiddenXInput.Length + 1);
            savedMiddenXInput[savedMiddenXInput.GetUpperBound(0)] = middenXInput.Text;
            Array.Resize(ref savedMiddenYInput, savedMiddenYInput.Length + 1);
            savedMiddenYInput[savedMiddenYInput.GetUpperBound(0)] = middenYInput.Text;
            Array.Resize(ref savedSchaalInput, savedSchaalInput.Length + 1);
            savedSchaalInput[savedSchaalInput.GetUpperBound(0)] = schaalInput.Text;
            Array.Resize(ref savedMaxAantalInput, savedMaxAantalInput.Length + 1);
            savedMaxAantalInput[savedMaxAantalInput.GetUpperBound(0)] = maxAantalInput.Text;
        }

        // Replace , to work on all computers, add culture to correct for system language
        CultureInfo ci = CultureInfo.CurrentCulture;
        string replacee = "~";
        string replacement = "~";
        if (ci.Name.ToString() == "nl-NL") { replacee = "."; replacement = ","; } 
        float x = float.Parse(middenXInput.Text.Replace(replacee, replacement));
        float y = float.Parse(middenYInput.Text.Replace(replacee, replacement));
        float schaal = float.Parse(schaalInput.Text.Replace(replacee, replacement));


        ImageBoxImage.Invalidate();
        // loop trough every pixel width and make it between a range between -2 and 2
        for (float i = 0; i <= imageSize; i++) {
            // Rows
            progressBar.Value = Convert.ToInt32((i / imageSize) * 100);
            // imageSize / 4 --> 400/4 = 100 --> i/100 gives range between -2 and 2
            // * schaal --> scretches image in both x and y to make it appear it is zoomed in
            double remappedPixelX = (-2 + (i / (imageSize/4)) + ((x/schaal) / (imageSize / 4))) * schaal;
            for (float j = 0; j <= 400; j++)
            {
                // Columns
                double remappedPixelY = (-2 + ((y /schaal) / (imageSize/4)) + (j / (imageSize / 4))) * schaal;
                int mandelPointGetal = calculateMandelBrotFromPoint(remappedPixelX, remappedPixelY);
                Brush pixelColor = Brushes.White;
                if (mandelPointGetal == maxAantal)
                {
                    pixelColor = Brushes.Black;
                } else {
                    Color color;
                    if (kleurZelfGekozen) {
                        /*  
                        hij mapt hier het mandelgetal naar de aangewezen kleur
                        op het moment dat je rood kiest verdeelt hij het mandelgetal (bijvoorbeeld 50) naar de range 0 tot 255
                        dus als het mandelgetal = maxAantal zal de kleur van de pixel 255 zijn en hoe lager het mandelgetal hoe lager de waarde van de kleur
                        */
                        int r = Convert.ToInt32(map(mandelPointGetal, 0, maxAantal, 0, kleur[0]));
                        int g = Convert.ToInt32(map(mandelPointGetal, 0, maxAantal, 0, kleur[1]));
                        int b = Convert.ToInt32(map(mandelPointGetal, 0, maxAantal, 0, kleur[2]));
                        color = Color.FromArgb(r, g, b);      
                    } else {
                        // als je de voorgekozen kleur kist rekent hij met setcolor de kleur uit
                        SetColor(mandelPointGetal);
                        color = Color.FromArgb(kleur[0], kleur[1], kleur[2]);      

                    }
                    // hier maak je de brush aan met de gekozen kleur
                    pixelColor = new SolidBrush(color);
                }

                ImageBoxDrawer.FillRectangle(pixelColor, i, j, 1, 1);

            }

        }
    }
    else
    {
        goBtn.Text = "Invalid";
    }
}

void SetColor(int value) {
    if (value != maxAantal) {
        // verdeel het mandelgetal (value) over 16 verschillende kleuren
        int colornr = value % 16;
        switch(colornr) {
            case 0: {
                // rgb(66, 30, 15)
                kleur[0] = 66;
                kleur[1] = 30;
                kleur[2] = 15;
                break;
            }
            case 1: {
                // rgb(25, 7, 26)
                kleur[0] = 25;
                kleur[1] = 7;
                kleur[2] = 26;
                break;

            }
            case 2: {
                // rgb(9, 1, 47)
                kleur[0] = 9;
                kleur[1] = 1;
                kleur[2] = 47;
                break;

            }
            case 3: {
                // rgb(4, 4, 73)
                kleur[0] = 4;
                kleur[1] = 4;
                kleur[2] = 73;
                break;

            }
            case 4: {
                // rgb( 12, 44, 138)
                kleur[0] = 12;
                kleur[1] = 44;
                kleur[2] = 138;
                break;

            }
            case 5: {
                // rgb(12, 44, 138)
                kleur[0] = 12;
                kleur[1] = 44;
                kleur[2] = 138;
                break;

            }
            case 6: {
                // rgb(24, 82, 177)
                kleur[0] = 24;
                kleur[1] = 82;
                kleur[2] = 177;
                break;

            }
            case 7: {
                // rgb(57, 125, 209)
                kleur[0] = 57;
                kleur[1] = 125;
                kleur[2] = 209;
                break;

            }
            case 8: {
                kleur[0] = 134;
                kleur[1] = 181;
                kleur[2] = 229;
                break;

            }
            case 9: {
                kleur[0] = 211;
                kleur[1] = 236;
                kleur[2] = 248;
                break;

            }
            case 10: {
                kleur[0] = 241;
                kleur[1] = 233;
                kleur[2] = 191;
                break;

            }
            case 11: {
                kleur[0] = 248;
                kleur[1] = 201;
                kleur[2] = 95;
                break;

            }
            case 12: {
                kleur[0] = 255;
                kleur[1] = 170;
                kleur[2] = 0;
                break;

            }
            case 13: {
                kleur[0] = 204;
                kleur[1] = 128;
                kleur[2] = 0;
                break;

            }
            case 14: {
                kleur[0] = 153;
                kleur[1] = 87;
                kleur[2] = 0;
                break;

            }
            case 15: {
                kleur[0] = 106;
                kleur[1] = 52;
                kleur[2] = 3;
                break;

            }

        }
    }
}

bool ValidateInputs(string middenX, string middenY, string schaal, string mAantal) {
    // Validate if all inputs from user are valid for calculation
    if (middenX == "" || middenY == "" || schaal == "" || mAantal == "") {
        return false;
    }
    return true;
}



int calculateMandelBrotFromPoint(double x, double y){

    int teller = 0;
    // variables
    // mandelBrot A & B start at 0
    double mandelBrotA = 0;
    double mandelBrotB = 0;
    // Give a maximum to the calculation
    
    for (int i = 0; i < maxAantal; i++)
    {
        if (CheckPythagoras(mandelBrotA, mandelBrotB, 0, 0))
        {
            double oldMandelBrotA = mandelBrotA;
            // CalculateMandelGetalX
            mandelBrotA = Math.Pow(mandelBrotA, 2) - Math.Pow(mandelBrotB, 2) + x;
            // CalculateMandelGetalY
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
    // als de muis dubbel is geklikt rekent hij uit wat de schaal, de x en de y coordinaten worden
    schaalInput.Text = (double.Parse(schaalInput.Text) / 2).ToString();
    middenXInput.Text = ((double.Parse(middenXInput.Text) + (e.X - 200) * double.Parse(schaalInput.Text))).ToString();
    middenYInput.Text = ((double.Parse(middenYInput.Text) + (e.Y - 200) * double.Parse(schaalInput.Text))).ToString();

    currentUndoPosition = 0;
    // de functie IterateThroughPixels() zal de waarde van schaalInput.Text, etc gebruiken
    IterateTroughPixels(kleurZelfGekozen);
}

//features

//enter on inputs
maxAantalInput.KeyDown += Input_KeyDown;
middenXInput.KeyDown += Input_KeyDown;
middenYInput.KeyDown += Input_KeyDown;
schaalInput.KeyDown += Input_KeyDown;

void Input_KeyDown(object sender, KeyEventArgs e)
{
    // roep de functie aan als je op enter hebt geklikt
    if (e.KeyCode == Keys.Enter) {
        IterateTroughPixels(kleurZelfGekozen);
    }
}

// map functie mapt een value uit een range[leftmin, leftmax] naar een andere range[rightmin, rightmax]
float map( float value, float leftMin, float leftMax, float rightMin, float rightMax )
{
  return rightMin + ( value - leftMin ) * ( rightMax - rightMin ) / ( leftMax - leftMin );
}

// Undo button
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
        IterateTroughPixels(kleurZelfGekozen);
    }
    
};

dropdown.SelectedIndexChanged += Dropdown_SelectedIndexChanged;
kleurDropDown.SelectedIndexChanged += kleurDropDown_SelectedIndexchanged;

void kleurDropDown_SelectedIndexchanged(object sender, EventArgs e) {
    // hier kies je zelf de waarden van de randen van de mandelbrot
    // deze worden in de functie IterateThroughPixel gemapt met de waarden van het mandelgetal
    if (presetsKleur[kleurDropDown.SelectedIndex] == "roze") {
        kleur[0] = 234;
        kleur[1] = 137;
        kleur[2] = 154;
        kleurZelfGekozen = true;
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "groen") {
        kleur[0] = 0;
        kleur[1] = 255;
        kleur[2] = 0;
        kleurZelfGekozen = true;
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "rood") {
        kleur[0] = 255;
        kleur[1] = 0;
        kleur[2] = 0;
        kleurZelfGekozen = true;
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "wit") {
        kleur[0] = 255;
        kleur[1] = 255;
        kleur[2] = 255;
        kleurZelfGekozen = true;
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "kleuren") {
        kleur[0] = 255;
        kleur[1] = 255;
        kleur[2] = 255;
        kleurZelfGekozen = false;
    }

    currentUndoPosition = 0;
    IterateTroughPixels(kleurZelfGekozen);
}

void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
{
    middenXInput.Text = middenXPresets[dropdown.SelectedIndex];
    middenYInput.Text = middenYPresets[dropdown.SelectedIndex];
    schaalInput.Text = schaalPresets[dropdown.SelectedIndex];
    currentUndoPosition = 0;
    IterateTroughPixels(kleurZelfGekozen);
}

Application.Run(scherm);