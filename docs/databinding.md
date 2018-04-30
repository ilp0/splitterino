# Data Binding

## Mitä on data binding

Data Binding on prosessi joka yhdistää ohjelman käyttöliittymän ja logiikan. Tieto voidaan sitoa käyttöliittymän kenttään, että kentän arvo muuttuu samalla kun kenttään sidottu tieto muuttuuu. Voit myös käyttää data bindingiä toiseen suuntaan, eli kun kentän arvo muuttuu, niin myös data muuttuu samalla.

## Mihin käytetään

Data Bindingiä voidaan käyttää käyttöliittymien päivittämisen helpottamiseen. Ilman Data Bindingiä kehittäjä joutuisi päivittämään manuaalisesti käyttöliittymän kentän, mutta data bindingin kentät päivittyy samalla kun data muuttuu.

## Miten

Data bindingiä voidaan käyttää WPF ympäristössä lisäämällä XAML tiedostoon esimerkiksi "TextBlock" elementin attribuuttiin "Text" lähde, josta data sidotaan, esimerkiksi:

```
<TextBox Name="SourceText" />
<TextBlock Text="{Binding Path=Text, ElementName=SourceText}" />
```

Tämä lisää käyttöliittymään TextBox elementin, joka on nimetty "SourceText" nimellä, ja TextBlock elementin, jonka attribuutissa "Text" määritellään datatyyppi sekä lähde.

Sama Kaksisuuntaisella bindingillä:

```
<TextBox Name="textBox" Text ="{Binding ElementName=listBox, Path=SelectedItem.Content, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" Background="{Binding ElementName=listBox, Path=SelectedItem.Content, Mode=OneWay}">
```

