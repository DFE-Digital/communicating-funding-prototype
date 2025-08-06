//
// For guidance on how to create filters see:
// https://prototype-kit.service.gov.uk/docs/filters
//

const govukPrototypeKit = require('govuk-prototype-kit')
const addFilter = govukPrototypeKit.views.addFilter

// Add your filters here
//
addFilter('asDetailedAllocationHeaderRows', fundingLines => {
    const periods = getDistributionPeriods(fundingLines);

    return [
        {
            text: 'Description',
        },
        ...Object.keys(periods).map(key => ({
            text: key,
            format: 'numeric',
        })),
        {
            text: "Sub-total",
            format: 'numeric',
        }
    ];
});
// =============================================================================
// Additional filter files
// =============================================================================

