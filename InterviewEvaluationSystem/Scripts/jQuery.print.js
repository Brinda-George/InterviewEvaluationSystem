// Create a jquery plugin that prints the given element.
jQuery.fn.print = function() {
    // NOTE: We are trimming the jQuery collection down to the
    // first element in the collection.
    if (this.size() > 1) {
        this.eq(0).print();
        return;
    } else if (!this.size()) {
        return;
    }

    // ASSERT: At this point, we know that the current jQuery
    // collection (as defined by THIS), contains only one
    // printable element.

    // Create a random name for the print frame.
    var strFrameName = ("printer-" + (new Date()).getTime());

    // Create an iFrame with the new name.
    var jFrame = $("<iframe name='" + strFrameName + "'>");

    // Hide the frame (sort of) and attach to the body.
    jFrame
    .css("width", "1px")
    .css("height", "1px")
    .css("position", "absolute")
    .css("left", "-9999px")
    .appendTo($("body:first"))
    ;

    // Get a FRAMES reference to the new frame.
    var objFrame = window.frames[strFrameName];

    // Get a reference to the DOM in the new frame.
    var objDoc = objFrame.document;

    // Grab all the style tags and copy to the new
    // document so that we capture look and feel of
    // the current document.

    // Create a temp document DIV to hold the style tags.
    // This is the only way I could find to get the style
    // tags into IE.
    //var jStyleDiv = $("<div>").append(
    //$("style").clone()
    //);

    var jStyleDiv = $("<div>").append(
        '<style type="text/css">.webgrid-header, .webgrid-header a {color: #000;text-align: left;text-decoration: none;}.webgrid-row-style, .webgrid-alternating-row {padding: 3px 7px 2px;}.ratingScale th:nth-child(1),.ratingScale td:nth-child(1){width:20%}.ratingScale th:nth-child(2),.ratingScale td:nth-child(2){width:10%}.ratingScale th:nth-child(3),.ratingScale td:nth-child(3){width:70%} .score td:first-child, .score th:first-child{width:30%;}.score td:not(:first-child), .score th:not(:first-child){width:15%;text-align:center;}.comment td:nth-child(3){width:40%px;text-align:left;}.comment td:nth-child(1),.comment td:nth-child(2),.comment td:nth-child(4){width:20%;text-align:left;}</style>');

    // Write the HTML for the document. In this, we will
    // write out the HTML of the current element.
    objDoc.open();
    objDoc.write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
    objDoc.write("<html>");
    objDoc.write("<body>");
    objDoc.write("<head>");
    objDoc.write("<title>");
    objDoc.write(document.title);
    objDoc.write("</title>");
    objDoc.write(jStyleDiv.html());
    objDoc.write("</head>");
    objDoc.write(this.html());
    objDoc.write("</body>");
    objDoc.write("</html>");
    objDoc.close();

    // Print the document.
    objFrame.focus();
    objFrame.print();

    // Have the frame remove itself in about a minute so that
    // we don't build up too many of these frames.
    setTimeout(
    function() {
        jFrame.remove();
    },
    (60 * 1000)
    );
}