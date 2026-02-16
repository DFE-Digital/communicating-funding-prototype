require('@dotenvx/dotenvx').config()

//
// For guidance on how to create routes see:
// https://prototype-kit.service.gov.uk/docs/create-routes
//

const govukPrototypeKit = require('govuk-prototype-kit')
const router = govukPrototypeKit.requests.setupRouter()

// Add your routes here
// render version 2.1
router.get("/prototype/v2.1/statement-from-api", async (req, res) => {
    const { fundingStreamId } = req.query;

    try {
        const statement = await fetchV2Statement(fundingStreamId);
        res.render("prototype/v2.1/statement-from-api", {
            statement
        });
    } catch (error) {
        console.error("Error fetching statement:", error);
        res.status(500).send()
    }
});

// render version 2.2
router.get("/prototype/v2.2/statement-from-api", async (req, res) => {
    const { fundingStreamId } = req.query;

    try {
        const statement = await fetchV2Statement(fundingStreamId);
        res.render("prototype/v2.2/statement-from-api", {
            statement
        });
    } catch (error) {
        console.error("Error fetching statement:", error);
        res.status(500).send()
    }
});

// render version mvp
router.get("/prototype/mvp/statement-from-api", async (req, res) => {
    const { fundingStreamId } = req.query;

    try {
        const statement = await fetchV2Statement(fundingStreamId);
        res.render("prototype/mvp/statement-from-api", {
            statement
        });
    } catch (error) {
        console.error("Error fetching statement:", error);
        res.status(500).send()
    }
});

// render version 3
router.get("/prototype/v3/statement-from-api", async (req, res) => {
    const { fundingStreamId } = req.query;

    try {
        const statement = await fetchV3Statement(fundingStreamId);
        res.render("prototype/v3/statement-from-api", {
            statement
        });
    } catch (error) {
        console.error("Error fetching statement:", error);
        res.status(500).send()
    }
});

//fetch v2 statement data from CFS test env
async function fetchV2Statement(fundingSteamId) {
    const url = `${process.env.PROTOTYPE_API_URL}/api/v2/statement/${fundingSteamId}`;
    console.log("Fetching statement from API for funding stream:", fundingSteamId, "URL:", url);
    const res = await fetch(url);
    return await res.json();
}


//fetch v3 statement data from CFS test env
async function fetchV3Statement(fundingSteamId) {
    const url = `${process.env.PROTOTYPE_API_URL}/api/v3/statement/${fundingSteamId}`;
    console.log("Fetching statement from API for funding stream:", fundingSteamId, "URL:", url);
    const res = await fetch(url);
    return await res.json();
}

//Calculations

// render Calculation Factors Data MVP v1
router.get("/prototype/calculations/v1/calculation-result-from-api", async (req, res) => {

    try {
        const calculationresult = await fetchV2CalculationResult();
        console.log(calculationresult)
        res.render("prototype/calculations/v1/calculation-result-from-api", {
            calculationresult
        });
    } catch (error) {
        console.error("Error fetching calculationresult:", error);
        res.status(500).send()
    }
});

//fetch v2 calculation result from CFS test env
async function fetchV2CalculationResult() {
    const url = `${process.env.PROTOTYPE_API_URL}/api/v2/calculationresult`;
    console.log("Fetching calculation result from API", "URL:", url);
    const res = await fetch(url);
    return await res.json();
}