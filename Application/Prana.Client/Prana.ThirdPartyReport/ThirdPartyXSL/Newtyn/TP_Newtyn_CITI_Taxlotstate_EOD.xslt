<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">  
	
    <ThirdPartyFlatFileDetailCollection>
   <ThirdPartyFlatFileDetail>
			 <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

			<RecordType>
				<xsl:value-of select ="'Record Type'"/>
			</RecordType>
			
			<TransactionType>
				<xsl:value-of select ="'Transaction Type'"/>
			</TransactionType>
			<ClientAccount>
				<xsl:value-of select ="'Client Account'"/>
			</ClientAccount>
			<AccountType>
				<xsl:value-of select ="'Account Type'"/>
			</AccountType>
			<ClientReference>
				<xsl:value-of select ="'Client Reference'"/>
			</ClientReference>
			<AmendNo>
				<xsl:value-of select ="'Amend No'"/>
			</AmendNo>
			<Instruction>
				<xsl:value-of select ="'Instruction'"/>
			</Instruction>
			<BrokerCode>
				<xsl:value-of select ="'Broker Code'"/>
			</BrokerCode>
			<SecurityId>
				<xsl:value-of select ="'Security Id'"/>
			</SecurityId>
			<SecurityIdType>
				<xsl:value-of select ="'Security Id Type'"/>
			</SecurityIdType>
			<TradeDate>
				<xsl:value-of select ="'TradeDate'"/>
			</TradeDate>
			<SettlementDate>
				<xsl:value-of select ="'SettlementDate'"/>
			</SettlementDate>
			<Quantity>
				<xsl:value-of select ="'Quantity'"/>
			</Quantity>
			<Price>
				<xsl:value-of select ="'Price'"/>
			</Price>
			<TradeCurrency>
				<xsl:value-of select ="'Trade Currency'"/>
			</TradeCurrency>
			<CommissionCode>
				<xsl:value-of select ="'Commission Code'"/>
			</CommissionCode>
			<Commission>
				<xsl:value-of select ="'Commission'"/>
			</Commission>
			<NetAmount>
				<xsl:value-of select ="'NetAmount'"/>
			</NetAmount>
			<MemoField1>
				<xsl:value-of select ="'MemoField 1'"/>
			</MemoField1>
			<MemoField2>
				<xsl:value-of select ="'MemoField 2'"/>
			</MemoField2>
			<RepoRate>
				<xsl:value-of select ="'Repo Rate'"/>
			</RepoRate>
			<RepoTerminationDate>
				<xsl:value-of select ="'RepoTermination Date'"/>
			</RepoTerminationDate>
			<RepoEndMoney>
				<xsl:value-of select ="'Repo End Money'"/>
			</RepoEndMoney>
			<RepoAccruedInterest>
				<xsl:value-of select ="'Repo Accrued Interest'"/>
			</RepoAccruedInterest>
			<RepoHaircut>
				<xsl:value-of select ="'Repo Haircut'"/>
			</RepoHaircut>
			<SettlementLocation>
				<xsl:value-of select ="'Settlement Location'"/>
			</SettlementLocation>
			<AccountNo>
				<xsl:value-of select ="'Account No'"/>
			</AccountNo>
			<AgentBankName>
				<xsl:value-of select ="'Agent Bank Name'"/>
			</AgentBankName>
			<AgentBankLocation>
				<xsl:value-of select ="'Agent Bank Location'"/>
			</AgentBankLocation>
			<AgentBankInstructions>
				<xsl:value-of select ="'Agent Bank Instructions'"/>
			</AgentBankInstructions>
			<FeeType1>
				<xsl:value-of select ="'Fee Type 1'"/>
			</FeeType1>
			<FeeValue1>
				<xsl:value-of select ="'Fee Value 1'"/>
			</FeeValue1>
			<FeeType2>
				<xsl:value-of select ="'Fee Type 2'"/>
			</FeeType2>
			<FeeValue2>
				<xsl:value-of select ="'Fee Value 2'"/>
			</FeeValue2>
			<FeeType3>
				<xsl:value-of select ="'Fee Type 3'"/>
			</FeeType3>
			<FeeValue3>
				<xsl:value-of select ="'Fee Value3'"/>
			</FeeValue3>
			<Strategy>
				<xsl:value-of select ="'Strategy'"/>
			</Strategy>
			<TaxlotId>
				<xsl:value-of select ="'Taxlot Id'"/>
			</TaxlotId>
			<PreFiguredIndicator>
				<xsl:value-of select ="'Pre Figured Indicator'"/>
			</PreFiguredIndicator>
			<BondInterest>
				<xsl:value-of select ="'Bond Interest'"/>
			</BondInterest>
			<BondPrincipal>
				<xsl:value-of select ="'Bond Principal'"/>
			</BondPrincipal>
			<ProcessingType>
				<xsl:value-of select ="'Processing Type'"/>
			</ProcessingType>
			<BlockId>
				<xsl:value-of select ="'Block Id'"/>
			</BlockId>
			<CFETSTradeID>
				<xsl:value-of select ="'CFETS Trade ID'"/>
			</CFETSTradeID>
			<ReservedField2>
				<xsl:value-of select ="'Reserved Field 2'"/>
			</ReservedField2>
			<ReservedField3>
				<xsl:value-of select ="'Reserved Field 3'"/>
			</ReservedField3>
			<BondFactor>
				<xsl:value-of select ="'Bond Factor'"/>
			</BondFactor>
			<Reserved>
				<xsl:value-of select ="'Reserved'"/>
			</Reserved>
			<ReservedField4>
				<xsl:value-of select ="'Reserved Field 4'"/>
			</ReservedField4>
			
			<ReservedField5>
				<xsl:value-of select ="'Reserved Field 5'"/>
			</ReservedField5>			

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
			 <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
		<xsl:variable name="varRecordType">
			<xsl:choose>
				<xsl:when test="AccountName='Citi Swap NP' or AccountName='Citi Swap NTE'">
					<xsl:value-of select="'Swap'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'Trade'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
				<RecordType>
				<xsl:value-of select ="$varRecordType"/>
			</RecordType>
			
			
			<xsl:variable name="varTransactionType">
        <xsl:choose>
            <xsl:when test="AccountName='Citi Swap NP' or AccountName='Citi Swap NTE'">
                <xsl:choose>
                    <xsl:when test="Side = 'Buy'">
                        <xsl:value-of select="'CFP'"/>
                    </xsl:when>
                    <xsl:when test="Side = 'Sell'">
                        <xsl:value-of select="'CFS'"/>
                    </xsl:when>
                </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
                <xsl:choose>
                    <xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
                        <xsl:value-of select="'BY'"/>
                    </xsl:when>
                    <xsl:when test="Side = 'Sell' or Side = 'Sell to Close'">
                        <xsl:value-of select="'SL'"/>
                    </xsl:when>
                    <xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
                        <xsl:value-of select="'SS'"/>
                    </xsl:when>
                    <xsl:when test="Side = 'Buy to Close'">
                        <xsl:value-of select="'BC'"/>
                    </xsl:when>
                    <xsl:when test="Side = 'Buy'">
                        <xsl:value-of select="'BY'"/>
                    </xsl:when>
                </xsl:choose>
            </xsl:otherwise>
        </xsl:choose>     
    </xsl:variable>
			<TransactionType>
				<xsl:value-of select="$varTransactionType"/>
			</TransactionType>
			
			<ClientAccount>
				<xsl:value-of select ="AccountNo"/>
			</ClientAccount>
			
			
	<xsl:variable name="varACType">
      <xsl:choose>
          <xsl:when test="AccountName='Citi Swap NP' or AccountName='Citi Swap NTE'">
              <xsl:value-of select="''"/>
          </xsl:when>
        <xsl:when test="Side = 'Buy' and Asset = 'Equity'">
          <xsl:value-of select="'Margin'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell' and Asset = 'Equity'">
          <xsl:value-of select="'Margin'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell short' and Asset = 'Equity'">
          <xsl:value-of select="'Short'"/>
        </xsl:when>
        <xsl:when test="Side = 'Buy to Close' and Asset = 'Equity'">
          <xsl:value-of select="'Short'"/>
        </xsl:when>       
      </xsl:choose>
    </xsl:variable>
			<AccountType>
				<xsl:value-of select ="$varACType"/>
			</AccountType>
			
			<ClientReference>
				<xsl:value-of select ="PBUniqueID"/>
			</ClientReference>
			<AmendNo>
				<xsl:value-of select ="''"/>
			</AmendNo>
			
          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'MOD'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CXL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
			<Instruction>
				<xsl:value-of select ="$varTaxlotState"/>
			</Instruction>
			<BrokerCode>
				<xsl:value-of select ="CounterParty"/>
			</BrokerCode>
			  
      <xsl:variable name="varSecurityID">
		<xsl:choose>         
          <xsl:when test="Asset = 'Equity' and not(contains(CurrencySymbol,'USD'))">
              <xsl:value-of select="SEDOLSymbol"/>
          </xsl:when>      
        <xsl:otherwise>
          <xsl:value-of select="Symbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
			<SecurityId>
				<xsl:value-of select ="$varSecurityID"/>
			</SecurityId>
			
		<xsl:variable name="varSecurityType">
          <xsl:choose>              
              <xsl:when test="Asset = 'Equity' and not(contains(CurrencySymbol,'USD'))">
                  <xsl:value-of select="'SEDOL'"/>
              </xsl:when>              
              <xsl:otherwise>
                  <xsl:value-of select="'TCKR'"/>
              </xsl:otherwise>
          </xsl:choose>
      </xsl:variable>
			<SecurityIdType>
				<xsl:value-of select ="$varSecurityType"/>
			</SecurityIdType>
			<TradeDate>
				<xsl:value-of select ="TradeDate"/>
			</TradeDate>
			<SettlementDate>
				<xsl:value-of select ="SettlementDate"/>
			</SettlementDate>
			<Quantity>
				<xsl:value-of select ="AllocatedQty"/>
			</Quantity>
			<Price>
				<xsl:value-of select ="AveragePrice"/>
			</Price>
			<TradeCurrency>
				<xsl:value-of select ="CurrencySymbol"/>
			</TradeCurrency>
			<CommissionCode>
				<xsl:value-of select ="'G'"/>
			</CommissionCode>
			<xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
			<Commission>
				  <xsl:value-of select="$varCommission"/>
			</Commission>
			<NetAmount>
				<xsl:value-of select ="NetAmount"/>
			</NetAmount>
			<MemoField1>
				<xsl:value-of select ="''"/>
			</MemoField1>
			<MemoField2>
				<xsl:value-of select ="''"/>
			</MemoField2>
			<RepoRate>
				<xsl:value-of select ="''"/>
			</RepoRate>
			<RepoTerminationDate>
				<xsl:value-of select ="''"/>
			</RepoTerminationDate>
			<RepoEndMoney>
				<xsl:value-of select ="''"/>
			</RepoEndMoney>
			<RepoAccruedInterest>
				<xsl:value-of select ="''"/>
			</RepoAccruedInterest>
			<RepoHaircut>
				<xsl:value-of select ="''"/>
			</RepoHaircut>
			<SettlementLocation>
				<xsl:value-of select ="''"/>
			</SettlementLocation>
			<AccountNo>
				<xsl:value-of select ="''"/>
			</AccountNo>
			<AgentBankName>
				<xsl:value-of select ="''"/>
			</AgentBankName>
			<AgentBankLocation>
				<xsl:value-of select ="''"/>
			</AgentBankLocation>
			<AgentBankInstructions>
				<xsl:value-of select ="''"/>
			</AgentBankInstructions>
			<FeeType1>
				<xsl:value-of select ="''"/>
			</FeeType1>
			<FeeValue1>
				<xsl:value-of select ="''"/>
			</FeeValue1>
			<FeeType2>
				<xsl:value-of select ="''"/>
			</FeeType2>
			<FeeValue2>
				<xsl:value-of select ="''"/>
			</FeeValue2>
			<FeeType3>
				<xsl:value-of select ="''"/>
			</FeeType3>
			<FeeValue3>
				<xsl:value-of select ="''"/>
			</FeeValue3>
			<Strategy>
				<xsl:value-of select ="''"/>
			</Strategy>
			<TaxlotId>
				<xsl:value-of select ="''"/>
			</TaxlotId>
			<PreFiguredIndicator>
				<xsl:value-of select ="''"/>
			</PreFiguredIndicator>
			<BondInterest>
				<xsl:value-of select ="''"/>
			</BondInterest>
			<BondPrincipal>
				<xsl:value-of select ="''"/>
			</BondPrincipal>
			<ProcessingType>
				<xsl:value-of select ="''"/>
			</ProcessingType>
			<BlockId>
				<xsl:value-of select ="''"/>
			</BlockId>
			<CFETSTradeID>
				<xsl:value-of select ="''"/>
			</CFETSTradeID>
			<ReservedField2>
				<xsl:value-of select ="''"/>
			</ReservedField2>
			<ReservedField3>
				<xsl:value-of select ="''"/>
			</ReservedField3>
			<BondFactor>
				<xsl:value-of select ="''"/>
			</BondFactor>
			<Reserved>
				<xsl:value-of select ="''"/>
			</Reserved>
			<ReservedField4>
				<xsl:value-of select ="''"/>
			</ReservedField4>
			
			<ReservedField5>
				<xsl:value-of select ="''"/>
			</ReservedField5>			

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>