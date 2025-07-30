using System.Collections;
using CommunicationsAlpha2025.Versions.V2;

namespace CommunicationsAlpha2025.Test.TestData;

public class FundingStreamTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return Augmentations
            .FundingStreamIdToName
            .Select(data => new object[] { data.Key })
            .GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}