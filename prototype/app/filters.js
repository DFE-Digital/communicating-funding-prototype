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

addFilter('asSummaryAllocationRows', fundingLines => {
    const periods = getDistributionPeriods(fundingLines);

    return Object.keys(periods).map(key => [
        { text: key },
        { text: currencyFormatter.format(periods[key]), format: "numeric" }
    ]);
})
//v2.1
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

// v2.2
addFilter('asV2DetailedAllocationHeaderRows', fundingLines => {
    const periods = getDistributionPeriods(fundingLines);

    return [
        {
            text: 'Funding line item',
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

addFilter('asDetailedAllocationRows', fundingLines => {
    const periodCount = Object.keys(getDistributionPeriods(fundingLines)).length;

    const rows = fundingLines.map(fundingLine => {
        const fundingLinePeriods = getDistributionPeriods([fundingLine]);
        const fundingLinePeriodsKeys = Object.keys(fundingLinePeriods);

        const row = [
            {
                text: fundingLine.name,
            }
        ];

        let subTotal = 0;
        for (let i = 0; i < periodCount; i++) {
            const periodKey = fundingLinePeriodsKeys[i];
            const value = fundingLinePeriods[periodKey] || 0;

            row.push({
                text: currencyFormatter.format(value),
                format: 'numeric',
            });

            subTotal += value;
        }

        row.push({
            text: currencyFormatter.format(subTotal),
            format: 'numeric',
        });

        return row;
    });

    const totalRow = [
        {
            html: `<strong>Total</strong>`
        }
    ];

    let subTotal = 0;
    for (let i = 0; i < periodCount; i++) {
        const periodKey = Object.keys(getDistributionPeriods(fundingLines))[i];
        const totalValue = fundingLines.reduce((sum, fundingLine) => {
            const fundingLinePeriods = getDistributionPeriods([fundingLine]);
            return sum + (fundingLinePeriods[periodKey] || 0);
        }, 0);

        subTotal += totalValue;

        totalRow.push({
            html: `<strong>${currencyFormatter.format(totalValue)}</strong>`,
            format: 'numeric',
        });
    }

    totalRow.push({
        html: `<strong>${currencyFormatter.format(subTotal)}</strong>`,
        format: 'numeric',
    }); 
    rows.push(totalRow);

    return rows;
});

function getDistributionPeriods(fundingLines) {
    const periods = {}
    for (const fundingLine of fundingLines) {
        if (!fundingLine?.distributionPeriods?.length) {
            continue;
        }

        for (const distributionPeriod of fundingLine.distributionPeriods) {
            const startFormatted = formatProfilePeriod(distributionPeriod.profilePeriods[0]); 
            const endFormatted = formatProfilePeriod(distributionPeriod.profilePeriods[distributionPeriod.profilePeriods.length - 1]);
            const distributionPeriodSummedValue = sumProfilePeriods(distributionPeriod.profilePeriods);

            periods[`${startFormatted} to ${endFormatted}`] =
                periods[`${startFormatted} to ${endFormatted}`] !== undefined
                    ? periods[`${startFormatted} to ${endFormatted}`] + distributionPeriodSummedValue
                    : distributionPeriodSummedValue;
        }
    }

    return periods;
}

function formatProfilePeriod(profilePeriod) {
    return `${profilePeriod.typeValue} ${profilePeriod.year}`; // e.g. August 2024
}

function sumProfilePeriods(profilePeriods) {
    return profilePeriods.reduce((sum, profilePeriod) => {
        return sum + (profilePeriod.value || 0);
    }, 0);
}

// Calculations

addFilter('asfundingLinesRows', fundingLines => {
    if (!fundingLines || !Array.isArray(fundingLines)) return [];

    return fundingLines
        .filter(line => line.value !== null) 
        .map(line => [
            { 
                text: line.name 
            },
            { 
                text: line.value.toLocaleString('en-GB', { 
                    style: 'currency', 
                    currency: 'GBP',
                    maximumFractionDigits: 0 
                }), 
                format: 'numeric' 
            }
        ]);
})