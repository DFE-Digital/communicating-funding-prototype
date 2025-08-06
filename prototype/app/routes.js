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
        const statement = await fetchStatement(fundingStreamId);
        res.render("prototype/v2.1/statement-from-api", {
            statement
        });
    } catch (error) {
        res.status(500).send()
    }
});

// render version 2.2
router.get("/prototype/v2.2/statement-from-api", async (req, res) => {
    const { fundingStreamId } = req.query;

    try {
        const statement = await fetchStatement(fundingStreamId);
        res.render("prototype/v2.2/statement-from-api", {
            statement
        });
    } catch (error) {
        res.status(500).send()
    }
});

//fetch statement data from CFS test env
async function fetchStatement(fundingSteamId) {
    const url = `${process.env.PROTOTYPE_API_URL}/api/v2/statement/${fundingSteamId}`;
    console.log("Fetching statement from API for funding stream:", fundingSteamId, "URL:", url);
    const res = await fetch(url);
    return await res.json();
}