using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
// Updated upstream
using System.Diagnostics;

using System.IO;
//Add Google sheet Apis (database)
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
//Add color because Google overwrites it by default
using Color = System.Drawing.Color;
using System.Collections.Generic;

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

Button savePresetBtn = new Button();
savePresetBtn.Text = "Save";
savePresetBtn.BackColor = Color.White;
//Button deletePresetBtn = new Button();
//deletePresetBtn.Text = "Delete";
//deletePresetBtn.BackColor = Color.White;

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
// Specify location of the elements
goBtn.Location = new Point(250, 100);
undoBtn.Location = new Point(310, 100);

savePresetBtn.Location = new Point(340, 134);
//deletePresetBtn.Location = new Point(395, 134);

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

// Specify Size of the Elements
goBtn.Size = new Size(50, 25);
undoBtn.Size = new Size(50, 25);

savePresetBtn.Size = new Size(50, 25);
//deletePresetBtn.Size = new Size(60, 25);

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

middenXInput.Text = "0";
middenYInput.Text = "0";
schaalInput.Text = "1";
maxAantalInput.Text = "100";

string[] presetsKleur = {"roze", "groen", "rood", "wit", "kleuren", "regenboog", "zwart wit"};
string[] presets = {"Spiraal", "Mandelbrot in mandelbrot", "Basis plaatje"};
string[] middenXPresets = { "36.98388671875", "-59.815625", "0" };
string[] middenYPresets = { "-67.1396484375", "-66.23906249999999", "0" };
string[] schaalPresets = { "0.000244140625", "0.00234375", "1" };

kleurDropDown.Items.AddRange(presetsKleur);
dropdown.Items.AddRange(presets);


// Add all elements to the screen
scherm.Controls.Add(goBtn);
scherm.Controls.Add(undoBtn);

scherm.Controls.Add(savePresetBtn);
//scherm.Controls.Add(deletePresetBtn);

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
 scherm.BackColor = Color.AntiqueWhite;
 scherm.FormBorderStyle = FormBorderStyle.FixedDialog;
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
string kleurZelfGekozen = "kleur";

// button event
goBtn.Click += GoBtn_Click;
void GoBtn_Click(object sender, EventArgs e)
{
    currentUndoPosition = 0;
    IterateTroughPixels(kleurZelfGekozen);
}

void IterateTroughPixels(string kleurZelfGekozen="kleur")
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
        // CultureInfo ci = CultureInfo.CurrentCulture;
        // string replacee = ",";
        // string replacement = ".";
        // if (ci.Name.ToString() == "nl-NL") { replacee = "."; replacement = ","; } 
        // float x = float.Parse(middenXInput.Text.Replace(replacee, replacement));
        // float y = float.Parse(middenYInput.Text.Replace(replacee, replacement));
        // float schaal = float.Parse(schaalInput.Text.Replace(replacee, replacement));

        float x = float.Parse(middenXInput.Text);
        float y = float.Parse(middenYInput.Text);
        float schaal = float.Parse(schaalInput.Text);


        ImageBoxImage.Invalidate();
        // loop trough every pixel width and make it between a range between -2 and 2
        for (float i = 0; i < imageSize; i++) {
            // Rows
            progressBar.Value = Convert.ToInt32((i / imageSize) * 100);
            // imageSize / 4 --> 400/4 = 100 --> i/100 gives range between -2 and 2
            // * schaal --> scretches image in both x and y to make it appear it is zoomed in
            double remappedPixelX = (-2 + (i / (imageSize/4)) + ((x/schaal) / (imageSize / 4))) * schaal;
            for (float j = 0; j < 400; j++)
            {
                // Columns
                double remappedPixelY = (-2 + ((y /schaal) / (imageSize/4)) + (j / (imageSize / 4))) * schaal;
                int mandelPointGetal = calculateMandelBrotFromPoint(remappedPixelX, remappedPixelY);
                //Brush pixelColor = Brushes.White;
                Color color;
                if (mandelPointGetal == maxAantal)
                {
                    //pixelColor = Brushes.Black;
                    color = Color.Black;
                } else {
                    
                    if (kleurZelfGekozen == "kleur") {
                        // als je de voorgekozen kleur kist rekent hij met setColorKleur de kleur uit
                        SetColorKleur(mandelPointGetal);
                        color = Color.FromArgb(kleur[0], kleur[1], kleur[2]);      
                    } else if (kleurZelfGekozen == "zwartwit") {
                        if (mandelPointGetal % 2 == 0) {
                            color = Color.Black;
                        } else {
                            color = Color.White;
                        }
                    } else if (kleurZelfGekozen == "regenboog") {
                        SetKleurRegenboog(mandelPointGetal);
                        color = Color.FromArgb(kleur[0], kleur[1], kleur[2]);
                    } else {
                        /*  
                        hij mapt hier het mandelgetal naar de aangewezen kleur
                        op het moment dat je rood kiest verdeelt hij het mandelgetal (bijvoorbeeld 50) naar de range 0 tot 255
                        dus als het mandelgetal = maxAantal zal de kleur van de pixel 255 zijn en hoe lager het mandelgetal hoe lager de waarde van de kleur
                        */
                        int r = Convert.ToInt32(map(mandelPointGetal, 0, maxAantal, 0, kleur[0]));
                        int g = Convert.ToInt32(map(mandelPointGetal, 0, maxAantal, 0, kleur[1]));
                        int b = Convert.ToInt32(map(mandelPointGetal, 0, maxAantal, 0, kleur[2]));
                        color = Color.FromArgb(r, g, b);      
                    }
                    // hier maak je de brush aan met de gekozen kleur
                    //pixelColor = new SolidBrush(color);
                }
                ImageBoxDrawing.SetPixel((int)(i), (int)(j), color);

            }

        }
    }
    else
    {
        goBtn.Text = "Invalid";
    }
}

void SetKleurRegenboog(int value) {
    if (value != maxAantal) {
        // verdeel het mandelgetal (value) over 16 verschillende kleuren
        int colornr = value % 7;
        switch(colornr) {
            case 0: {
                // rgb(127, 0, 255)
                kleur[0] = 127;
                kleur[1] = 0;
                kleur[2] = 255;
                break;

            }
            case 1: {
                // rgb(255, 0, 0)
                kleur[0] = 255;
                kleur[1] = 0;
                kleur[2] = 0;
                break;
            }
            case 2: {
                // rgb(255, 165, 0)
                kleur[0] = 255;
                kleur[1] = 165;
                kleur[2] = 0;
                break;

            }
            case 3: {
                // rgb(255, 255, 0)
                kleur[0] = 255;
                kleur[1] = 255;
                kleur[2] = 0;
                break;

            }
            case 4: {
                // rgb(0, 255, 0)
                kleur[0] = 0;
                kleur[1] = 255;
                kleur[2] = 0;
                break;

            }
            case 5: {
                // rgb(0, 0, 255)
                kleur[0] = 0;
                kleur[1] = 0;
                kleur[2] = 255;
                break;

            }
            case 6: {
                // rgb(75, 0, 130)
                kleur[0] = 75;
                kleur[1] = 0;
                kleur[2] = 130;
                break;

            }
        }
    }
}

int[] interpolation(int[] kleur1, int[] kleur2) {
    int[] returnKleur = {0,0,0};

    int r = Convert.ToInt32(Math.Floor((kleur2[0] - kleur1[0])* 0.5 + kleur1[0]));
    int g = Convert.ToInt32(Math.Floor((kleur2[1] - kleur1[1])* 0.5 + kleur1[1]));
    int b = Convert.ToInt32(Math.Floor((kleur2[2] - kleur1[2])* 0.5 + kleur1[2]));

    returnKleur[0] = r;
    returnKleur[1] = g;
    returnKleur[2] = b;
    return returnKleur;
}

void SetColorKleur(int value) {
    if (value != maxAantal) {
        // verdeel het mandelgetal (value) over 31 verschillende kleuren
        int colornr = value % 31;
        switch(colornr) {
            case 0: {
                // rgb(66, 30, 15)
                kleur[0] = 66;
                kleur[1] = 30;
                kleur[2] = 15;
                break;
            }
            case 1: {
                // om een zachtere overgang tussen de kleuren te krijgen rekent interpolation het gemiddelde van de twee kleuren uit
                int[] kleur1 = {66, 30, 15};
                int[] kleur2 = {25, 7, 26};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 2: {
                // rgb(25, 7, 26)
                kleur[0] = 25;
                kleur[1] = 7;
                kleur[2] = 26;
                break;

            }
            case 3: {
                int[] kleur1 = {25, 7, 26};
                int[] kleur2 = {9, 1, 47};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 4: {
                // rgb(9, 1, 47)
                kleur[0] = 9;
                kleur[1] = 1;
                kleur[2] = 47;
                break;

            }
            case 5: {
                int[] kleur1 = {9, 1, 47};
                int[] kleur2 = {4, 4, 73};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 6: {
                // rgb(4, 4, 73)
                kleur[0] = 4;
                kleur[1] = 4;
                kleur[2] = 73;
                break;

            }
            case 7: {
                int[] kleur1 = {4, 4, 73};
                int[] kleur2 = {12, 44, 138};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 8: {
                // rgb( 12, 44, 138)
                kleur[0] = 12;
                kleur[1] = 44;
                kleur[2] = 138;
                break;

            }
            case 9: {
                int[] kleur1 = {12, 44, 138};
                int[] kleur2 = {12, 44, 138};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 10: {
                // rgb(12, 44, 138)
                kleur[0] = 12;
                kleur[1] = 44;
                kleur[2] = 138;
                break;

            }
            case 11: {
                int[] kleur1 = {12, 44, 138};
                int[] kleur2 = {24, 82, 177};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 12: {
                // rgb(24, 82, 177)
                kleur[0] = 24;
                kleur[1] = 82;
                kleur[2] = 177;
                break;

            }
            case 13: {
                int[] kleur1 = {24, 82, 177};
                int[] kleur2 = {57, 125, 209};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 14: {
                // rgb(57, 125, 209)
                kleur[0] = 57;
                kleur[1] = 125;
                kleur[2] = 209;
                break;

            }
            case 15: {
                int[] kleur1 = {57, 125, 209};
                int[] kleur2 = {134, 181, 229};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 16: {
                kleur[0] = 134;
                kleur[1] = 181;
                kleur[2] = 229;
                break;
            }
            case 17: {
                int[] kleur1 = {134, 181, 229};
                int[] kleur2 = {211, 236, 248};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 18: {
                kleur[0] = 211;
                kleur[1] = 236;
                kleur[2] = 248;
                break;

            }
            case 19: {
                int[] kleur1 = {221, 236, 248};
                int[] kleur2 = {241, 233, 191};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 20: {
                kleur[0] = 241;
                kleur[1] = 233;
                kleur[2] = 191;
                break;
            }
            case 21: {
                int[] kleur1 = {241, 233, 191};
                int[] kleur2 = {248, 201, 95};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 22: {
                kleur[0] = 248;
                kleur[1] = 201;
                kleur[2] = 95;
                break;
            }
            case 23: {
                int[] kleur1 = {248, 201, 95};
                int[] kleur2 = {255, 170, 0};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 24: {
                kleur[0] = 255;
                kleur[1] = 170;
                kleur[2] = 0;
                break;

            }
            case 25: {
                int[] kleur1 = {255, 170, 0};
                int[] kleur2 = {204, 128, 0};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 26: {
                kleur[0] = 204;
                kleur[1] = 128;
                kleur[2] = 0;
                break;

            }
            case 27: {
                int[] kleur1 = {204, 128, 0};
                int[] kleur2 = {153, 87, 0};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 28: {
                kleur[0] = 153;
                kleur[1] = 87;
                kleur[2] = 0;
                break;

            }
            case 29: {
                int[] kleur1 = {153, 87, 0};
                int[] kleur2 = {106, 52, 3};
                kleur = interpolation(kleur1, kleur2);
                break;
            }
            case 30: {
                kleur[0] = 106;
                kleur[1] = 52;
                kleur[2] = 3;
                break;

            }
            case 31: {
                int[] kleur1 = {66, 30, 15};
                int[] kleur2 = {106, 52, 3};
                kleur = interpolation(kleur1, kleur2);
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
ImageBoxImage.MouseClick += ImageBoxImage_MouseRichtClick;

void ImageBoxImage_MouseRichtClick(object sender, MouseEventArgs e) {

    CultureInfo ci = CultureInfo.CurrentCulture;
    string replacee = ",";
    string replacement = ".";
    if (ci.Name.ToString() == "nl-NL") { replacee = "."; replacement = ","; }

    if (e.Button == System.Windows.Forms.MouseButtons.Right) {
        // als de rechtermuis is geklikt rekent hij uit wat de schaal, de x en de y coordinaten worden
        schaalInput.Text = (double.Parse(schaalInput.Text.Replace(replacee, replacement)) * 2).ToString();
        middenXInput.Text = ((double.Parse(middenXInput.Text.Replace(replacee, replacement)) + (e.X - 200) * double.Parse(schaalInput.Text.Replace(replacee, replacement)))).ToString();
        middenYInput.Text = ((double.Parse(middenYInput.Text.Replace(replacee, replacement)) + (e.Y - 200) * double.Parse(schaalInput.Text.Replace(replacee, replacement)))).ToString();

        currentUndoPosition = 0;
        // de functie IterateThroughPixels() zal de waarde van schaalInput.Text, etc gebruiken
        IterateTroughPixels(kleurZelfGekozen);
    }
}

void ImageBoxImage_MouseDoubleClick(object sender, MouseEventArgs e)
{
    // als de muis dubbel is geklikt rekent hij uit wat de schaal, de x en de y coordinaten worden
    CultureInfo ci = CultureInfo.CurrentCulture;
    string replacee = ",";
    string replacement = ".";
    if (ci.Name.ToString() == "nl-NL") { replacee = "."; replacement = ","; }

    schaalInput.Text = (double.Parse(schaalInput.Text.Replace(replacee, replacement)) / 2).ToString();
    middenXInput.Text = ((double.Parse(middenXInput.Text.Replace(replacee, replacement)) + (e.X - 200) * double.Parse(schaalInput.Text.Replace(replacee, replacement)))).ToString();
    middenYInput.Text = ((double.Parse(middenYInput.Text.Replace(replacee, replacement)) + (e.Y - 200) * double.Parse(schaalInput.Text.Replace(replacee, replacement)))).ToString();

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
        kleurZelfGekozen = "ja";
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "groen") {
        kleur[0] = 0;
        kleur[1] = 255;
        kleur[2] = 0;
        kleurZelfGekozen = "ja";
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "rood") {
        kleur[0] = 255;
        kleur[1] = 0;
        kleur[2] = 0;
        kleurZelfGekozen = "ja";
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "wit") {
        kleur[0] = 255;
        kleur[1] = 255;
        kleur[2] = 255;
        kleurZelfGekozen = "ja";
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "kleuren") {
        kleur[0] = 255;
        kleur[1] = 255;
        kleur[2] = 255;
        kleurZelfGekozen = "kleur";
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "zwart wit") {
        kleurZelfGekozen = "zwartwit";
    } else if (presetsKleur[kleurDropDown.SelectedIndex] == "regenboog") {
        kleurZelfGekozen = "regenboog";
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


//=============DATABASE=================

bool databaseEnabled = true;



//authenticate

string[] Scopes = { SheetsService.Scope.Spreadsheets };
string ApplicationName = "Mandelbrot"; //GOOGLE SHEET NAME --> DO NOT CHANGE
string SpreadsheetId = "1J1m0vFLmtPdknE6-NqQwQxenV1rqXHS7N_3rBAFBGmc"; //GOOGLE SHEET ID (URL)
string sheet = "Presets"; //Shown in lower tabs of Google Sheets
SheetsService service = null;


ConnectToDatabase();

//FOR MORE INFORMATION ABOUT THE SHEET USED AND ACCESS TO IT: https://docs.google.com/spreadsheets/d/1J1m0vFLmtPdknE6-NqQwQxenV1rqXHS7N_3rBAFBGmc/edit?usp=sharing



void ConnectToDatabase()
{
    // try
    // {
        GoogleCredential credential;
        using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        }


        service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });

        //call first time to initialize the presets into the dropdown
        ReadTemplates();
    // }
    // catch {
    //     MessageBox.Show("Database connection was not successfull because you don't have the credentials on your device. This feature will be disabled for now");
    //     databaseEnabled = false;
    // }
}

void ReadTemplates() {
    //database is not disabled, so create new presetLists
    string[] presetsPre = {  };
    string[] middenXPresetsPre = {  };
    string[] middenYPresetsPre = {  };
    string[] schaalPresetsPre = {  };

    //setup range of where to read
    string sheetRange = "A2:D";
    var range = $"{sheet}!{sheetRange}";

    //make request, get response from range and store it in values
    var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
    var response = request.Execute();
    var values = response.Values;

    if (values != null && values.Count > 0)
    {
        foreach (var row in values)
        {
            if (row[0] != null)
            {
                //Column 0 = presetName, 1 = midX, 2 = midY, 3 = scale
                string presetName = row[0].ToString();
                string midX = row[1].ToString();
                string midY = row[2].ToString();
                string scale = row[3].ToString();

                Debug.WriteLine(presetsPre.Length);

                //push each value to local array
                Array.Resize(ref presetsPre, presetsPre.Length + 1);
                presetsPre[presetsPre.Length - 1] = presetName;
                Array.Resize(ref middenXPresetsPre, middenXPresetsPre.Length + 1);
                middenXPresetsPre[middenXPresetsPre.Length - 1] = midX;
                Array.Resize(ref middenYPresetsPre, middenYPresetsPre.Length + 1);
                middenYPresetsPre[middenYPresetsPre.Length - 1] = midY;
                Array.Resize(ref schaalPresetsPre, schaalPresetsPre.Length + 1);
                schaalPresetsPre[schaalPresetsPre.Length - 1] = scale;

                //add to ui element
                presets = presetsPre;
                middenXPresets = middenXPresetsPre;
                middenYPresets = middenYPresetsPre;
                schaalPresets = schaalPresetsPre;

                dropdown.Items.Clear();
                dropdown.Items.AddRange(presetsPre);
            } else { break; }
        }
    }
}

int GetLastRowOfRange(string sheetRange)
{
    int lastRow = 2; //start at row 2, (not index 2)
    var range = $"{sheet}!{sheetRange}";
    var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

    var response = request.Execute();
    var values = response.Values;
    if (values != null && values.Count > 0)
    {

        //ROWS
        foreach (var row in values)
        {
            try
            {
                lastRow += 1;
            }
            catch
            {
                MessageBox.Show("The range is not defined", "There was an error");
            }

        }
        return lastRow;
    }
    else
    {
        Console.WriteLine("No Data Was Found.");
        return 0;
    }
}

void UpdateEntry(string sheetRange, string objectToUpdate)
{
    var range = $"{sheet}!{sheetRange}";
    var valueRange = new ValueRange();

    var objectList = new List<object>() { { objectToUpdate } };
    valueRange.Values = new List<IList<object>> { objectList };

    var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
    var updateResponse = updateRequest.Execute();

}

void DeleteEntry(string sheetRange)
{
    var range = $"{sheet}!{sheetRange}";
    var requestBody = new ClearValuesRequest();

    var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, SpreadsheetId, range);
    var deleteResponse = deleteRequest.Execute();
}


//Database Buttons

savePresetBtn.Click += SavePresetBtn_Click;

void SavePresetBtn_Click(object sender, EventArgs e)
{

    if (databaseEnabled)
    {
        if (ValidateInputs(middenXInput.Text, middenYInput.Text, schaalInput.Text, maxAantalInput.Text))
        {
            string nextRow = GetLastRowOfRange("A2:D").ToString();
            UpdateEntry("A" + nextRow, dropdown.Text);
            UpdateEntry("B" + nextRow, middenXInput.Text);
            UpdateEntry("C" + nextRow, middenYInput.Text);
            UpdateEntry("D" + nextRow, schaalInput.Text);

            MessageBox.Show("Preset successfully added to database!");
            ReadTemplates();
        }
    }
    else
    {
        MessageBox.Show("Database disabled");
    }
}

/*deletePresetBtn.Click += DeletePresetBtn_Click;

void DeletePresetBtn_Click(object sender, EventArgs e)
{
    if (databaseEnabled)
    {
        if (ValidateInputs(middenXInput.Text, middenYInput.Text, schaalInput.Text, maxAantalInput.Text))
        {
            string rowToDelete = (dropdown.TabIndex + 2).ToString(); //+2 to correct for index and first unavailable row
            DeleteEntry("A" + rowToDelete);

            MessageBox.Show("Preset successfully deleted to database!");
            ReadTemplates();

        }
    }
    else
    {
        MessageBox.Show("Database disabled");
    }
}*/


Application.Run(scherm);