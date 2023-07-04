﻿using Models.Interfaces.ApplicationServices.AccountService;
using Models.Interfaces.DomainServices.JsonSerialiser;
using Models.Models.ApplicationServices.AccountService;
using Models.Models.InvestecServices;

namespace Application.AccountService;

public class AccountService : IAccountService
{
  private readonly IJsonSerialiser jsonSerialiser;
  public AccountService(IJsonSerialiser jsonSerialiser)
  {
    this.jsonSerialiser = jsonSerialiser;
  }

  public async Task<List<AccountIdentity>> GetAccountIdsAsync(string accessToken, string uriBase = "https://openapi.investec.com")
  {
    // Set up the request
    var request = new HttpRequestMessage(HttpMethod.Get, $"{uriBase}/za/pb/v1/accounts");
    request.Headers.Add("Accept", "application/json");
    request.Headers.Add("Authorization", $"Bearer {accessToken}");

    // Make the API Call
    var client = new HttpClient();
    var response = await client.SendAsync(request);

    // Extract the response
    var responseContent = await response.Content.ReadAsStringAsync();
    var deserialisedResponse = jsonSerialiser.DeserializeObject<GetAccountList>(responseContent);
    var accountIds = deserialisedResponse.Data.Accounts
      .Select(accountData => new AccountIdentity{AccountId = accountData?.AccountId, ProductName = accountData?.ProductName})
      .Where(accountIdentity => string.IsNullOrEmpty(accountIdentity.AccountId) == false)
      .ToList();
    return accountIds;
  }

  public Task<List<AccountBalance>> GetAccountBalancesAsync(List<AccountIdentity> accountIds, string accessToken, string uriBase = "https://openapi.investec.com")
  {
    throw new NotImplementedException();
  }
}