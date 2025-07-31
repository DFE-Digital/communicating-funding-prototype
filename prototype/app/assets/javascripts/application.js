//
// For guidance on how to add JavaScript see:
// https://prototype-kit.service.gov.uk/docs/adding-css-javascript-and-images
//

window.GOVUKPrototypeKit.documentReady(() => {
  // Add JavaScript here
})


Handlebars.registerHelper('formatAsCurrency', function (value) {
    if (value === null || value === undefined) {
        return "£0.00";
    }
    return "£" + parseFloat(value).toFixed(2);
})