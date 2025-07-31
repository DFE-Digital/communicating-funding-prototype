require('@dotenvx/dotenvx').config()

//
// For guidance on how to create routes see:
// https://prototype-kit.service.gov.uk/docs/create-routes
//

const govukPrototypeKit = require('govuk-prototype-kit')
const router = govukPrototypeKit.requests.setupRouter()

// Add your routes here

router.get("/prototype/v2/statement-from-api", async (req, res) => {
    const { fundingStreamId } = req.query;

    const statement = await fetchStatement(fundingStreamId);
    res.render("prototype/v2/statement-from-api", {
        statement
    });
});

async function fetchStatement(fundingSteamId) {
    const url = `${process.env.PROTOTYPE_API_URL}/api/v2/statement/${fundingSteamId}`;
    console.log("Fetching statement from API for funding stream:", fundingSteamId, "URL:", url);
    const res = await fetch(url);
    return await res.json();
}