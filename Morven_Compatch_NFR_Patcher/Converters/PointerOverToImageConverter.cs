/*=============================================================================================*
*   Class: PointerOverToImageConverter
*
*   Description:
*       This converter is used to dynamically switch between two different question mark images
*       based on whether the mouse pointer is hovering over a control (typically a Button).
*       It implements IValueConverter from Avalonia, converting a boolean value (usually the Button's
*       IsPointerOver property) into a Bitmap image.
*
*       When the bound value is true (indicating that the pointer is over the control), this 
*       converter returns a Bitmap loaded from the 'question_mark_hover.png' resource. Otherwise,
*       it returns a Bitmap loaded from the default 'question_mark.png' resource.
*
*       The converter uses the static AssetLoader class (introduced in Avalonia 11) to open a stream
*       for the resource URI and create a Bitmap. Both image files must be correctly located and marked
*       as AvaloniaResource.
*
*   Usage:
*       Bind the Source property of an Image control to the IsPointerOver property of its parent
*       Button, using this converter. For example:
*
*     Source="{Binding IsPointerOver, RelativeSource={RelativeSource AncestorType=Button}, 
*              Converter={StaticResource PointerOverToImageConverter}}"
*
*   Dependencies:
*       - Avalonia.Data.Converters.IValueConverter for data conversion.
*       - Avalonia.Media.Imaging.Bitmap to represent images.
*       - Avalonia.Platform.AssetLoader for loading images from the embedded resources.
*
*   Note:
*       The ConvertBack method is not implemented, as this converter is intended for one-way binding only.
*
*=============================================================================================*/

#nullable enable
using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Morven_Compatch_NFR_Patcher.Converters
{
    // Converts a boolean value into an image. Depending on the value, it will either return question_mark image or question_mark_hover image.
    public class PointerOverToImageConverter : IValueConverter
    {
        /*=============================================================================================*
         * Function: Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
         *
         * Description: This function converts a boolean value indicating whether the pointer is over a 
         *              control (typically the Button's IsPointerOver property) into a Bitmap image.
         *              When the pointer is over the control, it returns the hover image; otherwise,
         *              it returns the default image.
         *
         * @var value
         *      The input from the binding, expected to be a boolean that is true if the pointer is over the control.
         * @var targetType
         *      The target type for the conversion (typically an IImage).
         * @var parameter
         *      An optional parameter (unused in this converter).
         * @var culture
         *      The culture information for the conversion.
         *
         * Returns:
         *      A Bitmap created from the appropriate asset resource URI.
         *=============================================================================================*/

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Set the default URI for the normal question mark image.
            string uriString = "avares://Morven_Compatch_NFR_Patcher/Assets/images/question_mark.png";

            // If the binding value is true (i.e., the pointer is over the control), change the URI to the hover version.
            if (value is bool isPointerOver && isPointerOver)
            {
                uriString = "avares://Morven_Compatch_NFR_Patcher/Assets/images/question_mark_hover.png";
            }

            // Create a new Uri from the string.
            var uri = new Uri(uriString);

            // Open a stream to the asset using the static AssetLoader.
            using var stream = AssetLoader.Open(uri);

            // Create and return a new Bitmap from the stream.
            return new Bitmap(stream);
        }

        /*=============================================================================================*
         * Function: ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
         *
         * Description: This function is intended to convert a Bitmap image (IImage) back to a boolean
         *              value. However, this converter is designed for one-way binding only.
         *              Therefore, this function is not implemented.
         *
         * @var value
         *      The value from the binding that should be converted back (unused).
         * @var targetType
         *      The target type for the conversion (unused).
         * @var parameter
         *      An optional parameter (unused).
         * @var culture
         *      The culture information for the conversion (unused).
         *
         * Returns:
         *      Throws a NotImplementedException because the conversion back is not supported.
         *=============================================================================================*/

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

