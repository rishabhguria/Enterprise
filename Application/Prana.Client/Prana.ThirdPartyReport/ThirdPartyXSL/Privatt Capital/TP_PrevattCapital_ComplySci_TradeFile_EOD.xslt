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
			
			<TradeDate>
				<xsl:value-of select="'TradeDate'"/>
			</TradeDate>

			<Symbol>
				<xsl:value-of select="'Symbol'"/>
			</Symbol>

			<CUSIP>
				<xsl:value-of select="'CUSIP'"/>
			</CUSIP>

			<SEDOL>
				<xsl:value-of select="'SEDOL'"/>
			</SEDOL>

			<ISIN>
				<xsl:value-of select="'ISIN'"/>
			</ISIN>

			<Quantity>
				<xsl:value-of select="'Quantity'"/>
			</Quantity>

			<PricePerShare>
				<xsl:value-of select="'PricePerShare'"/>
			</PricePerShare>

			<CurrencyCode>
				<xsl:value-of select="'CurrencyCode'"/>
			</CurrencyCode>

			<IncludeAllValor>
				<xsl:value-of select="'IncludeAllValor'"/>
			</IncludeAllValor>

			<IncludeAllCUSIP>
				<xsl:value-of select="'IncludeAllCUSIP'"/>
			</IncludeAllCUSIP>


			<IncludeAllGKKey>
				<xsl:value-of select="'IncludeAllGKKey'"/>
			</IncludeAllGKKey>

			<Groups>
				<xsl:value-of select="'Groups'"/>
			</Groups>

			<CompanyName>
				<xsl:value-of select="'CompanyName'"/>
			</CompanyName>

			<Description>
				<xsl:value-of select="'Description'"/>
			</Description>

			<SecurityType>
				<xsl:value-of select="'SecurityType'"/>
			</SecurityType>

			<TradeFilled>
				<xsl:value-of select="'TradeFilled'"/>
			</TradeFilled>

			<CreateIfNotFound>
				<xsl:value-of select="'CreateIfNotFound'"/>
			</CreateIfNotFound>

			<EntityID>
				<xsl:value-of select="EntityID"/>
			</EntityID>

		</ThirdPartyFlatFileDetail>
   <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty != 'CorpAction' or  CounterParty != 'Transfer']">

        <ThirdPartyFlatFileDetail>
		
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
          <xsl:variable name="Trade_Day" select="substring(TradeDate,4,2)"/>
          <xsl:variable name="Trade_Month" select="substring(TradeDate,1,2)"/>
          <xsl:variable name="Trade_Year" select="substring(TradeDate,7,4)"/>
          <TradeDate>
            <xsl:value-of select="concat($Trade_Month,'/',$Trade_Day,'/',$Trade_Year)"/>
          </TradeDate>

			<Symbol>
				<xsl:value-of select="Symbol"/>
			</Symbol>

			<CUSIP>
				<xsl:value-of select="CUSIP"/>
			</CUSIP>
			
			<SEDOL>
				<xsl:value-of select="SEDOL"/>
			</SEDOL>

			<ISIN>
				<xsl:value-of select="ISIN"/>
			</ISIN>
			
			<Quantity>
				<xsl:value-of select="AllocatedQty"/>
			</Quantity>

			<PricePerShare>
				<xsl:value-of select="AveragePrice"/>
			</PricePerShare>

			<CurrencyCode>
				<xsl:value-of select="CurrencySymbol"/>
			</CurrencyCode>

			<IncludeAllValor>
				<xsl:value-of select="''"/>
			</IncludeAllValor>
			
			<IncludeAllCUSIP>
				<xsl:value-of select="''"/>
			</IncludeAllCUSIP>
			
			
			<IncludeAllGKKey>
				<xsl:value-of select="''"/>
			</IncludeAllGKKey>
			
			<Groups>
				<xsl:value-of select="'All Employees'"/>
			</Groups>

			<CompanyName>
				<xsl:value-of select="''"/>
			</CompanyName>
			
			<Description>
				<xsl:value-of select="FullSecurityName"/>
			</Description>
			
			<SecurityType>
				<xsl:value-of select="Asset"/>
			</SecurityType>
			
			<TradeFilled>
				<xsl:value-of select="''"/>
			</TradeFilled>
			
			<CreateIfNotFound>
				<xsl:value-of select="''"/>
			</CreateIfNotFound>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>