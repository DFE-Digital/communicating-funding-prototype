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

// Takes in a fundingLines Object, of which contains fundingLines and calculations
// Here we roll through the Object recursively, outputting all funding lines and calculations in the appropriate hierachy
addFilter('asfundingLinesRows', fundingAndCalculationLines => {
    const rows = [];

    formatCalculationFactorRows(fundingAndCalculationLines, rows);
    return rows;
});

function formatCalculationFactorRows(fundingAndCalculationLines, rows, level = 0) {
    if (!fundingAndCalculationLines || !Array.isArray(fundingAndCalculationLines)) return;

    fundingAndCalculationLines.forEach(item => {
        const isDuplicateName = rows.some(row => row[0].text === item.name);
        let rowAdded = false;

        if (item.value !== null && item.value !== undefined && !isDuplicateName) {
        

        if (!isDuplicateName && item.value !== null && item.value !== undefined) {
        // Format based on the type
        let formattedValue = item.value;
        //If valueFormat does not exist as a property, we are on the fundingLine, which is always a currency value
        if (item.valueFormat === 'Currency' || !item.valueFormat) { 
            formattedValue = new Intl.NumberFormat('en-GB', { 
            style: 'currency', 
            currency: 'GBP',
            maximumFractionDigits: 0 
            }).format(item.value);
        } else if (item.valueFormat === 'Percentage') {
            formattedValue = item.value + '%';
        } else {
            formattedValue = item.value.toLocaleString('en-GB');
        }
        
        // Indent, based on level in the JSON hierachy
        const indentationClass = level > 0 ? `govuk-!-padding-left-${Math.min(level * 3, 9)}` : "";

        rows.push([
            { 
            text: item.name,
            classes: indentationClass
            },
            { 
            text: formattedValue, 
            format: 'numeric' 
            }
        ]);
        rowAdded = true;
        }
        }

        //If the row was skipped, thanks to a dupe or null, don't iterate the level, to maintain indentation
        const nextLevel = rowAdded ? level + 1 : level;

        //Recurse if further calculations exist, on our current item
        if (item.calculations && item.calculations.length > 0) {
        formatCalculationFactorRows(item.calculations, rows, nextLevel);
        }

        //Recurse if further funding lines exist, on our current item
        if (item.fundingLines && item.fundingLines.length > 0) {
        formatCalculationFactorRows(item.fundingLines, rows, nextLevel);
        }
    });
}