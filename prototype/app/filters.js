//
// For guidance on how to create filters see:
// https://prototype-kit.service.gov.uk/docs/filters
//

const govukPrototypeKit = require('govuk-prototype-kit')
const addFilter = govukPrototypeKit.views.addFilter

// Add your filters here

const currencyFormatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: 'GBP',
});

addFilter('formatAsCurrency', (value) => {
    return currencyFormatter.format(value);
})

addFilter('sentenceCaseLower', (str) => {
    return str.charAt(0).toLowerCase() + str.slice(1);
});

addFilter('formatAsDate', (date) => {
    // format as DD MMMM YYYY
    const options = { day: '2-digit', month: 'long', year: 'numeric' };
    return new Date(date).toLocaleDateString('en-GB', options);
});