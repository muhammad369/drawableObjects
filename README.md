# drawableObjects
a drawing library for .Net works on winForms PictureBox 

it may be used to implement complex interactive charts

it assigns a cache image for each layer and composite drawable to optimize redrawing of the visual tree

#Example
```cs
DrawingController dc = new AnimationDrawingController(pictureBox1, 10);

//whatever the dimensions of the pictureBox control, 
//pass your own preffered width and height to the layer constructor and every inner shape will be scaled accordingly

Layer l = new Layer(200, 100, dc);

//create and add shapes to the layer

Rect b = new Rect(10, 10, 20, 20, new SolidBrush(Color.FromArgb(100, Color.Green)), null);

l.add(b);

//assign events

b.clicked += new Drawable.onClick(b_clicked);
 
//you must register the shape as clickable
dc.registerClickable(b);
 
void b_clicked(Drawable sender, float vx, float vy)
{
    sender.setText("I've been clicked", SystemFonts.DefaultFont, Brushes.Black);
}


```
