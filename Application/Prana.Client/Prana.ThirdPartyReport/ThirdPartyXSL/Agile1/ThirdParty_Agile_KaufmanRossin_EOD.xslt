<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
		  <xsl:if test ="AccountName = 'Clover Street'">
			  <ThirdPartyFlatFileDetail>

				  <!--for system internal use-->

				  <FileHeader>
					  <xsl:value-of select ="'false'"/>
				  </FileHeader>

				  <FileFooter>
					  <xsl:value-of select ="'false'"/>
				  </FileFooter>

				  <!--for system internal use-->
				  <RowHeader>
					  <xsl:value-of select ="'true'"/>
				  </RowHeader>

				  <!--for system internal use-->
				  <TaxLotState>
					  <xsl:value-of select="TaxLotState"/>
				  </TaxLotState>

				  <xsl:variable name="Prana_FundName">
					  <xsl:value-of select="AccountName"/>
				  </xsl:variable>


				  <TRADE_DATE>
					  <xsl:value-of select="TradeDate"/>
				  </TRADE_DATE>

				  <SETTLE_DATE>
					  <xsl:value-of select="SettlementDate"/>
				  </SETTLE_DATE>

				  <TRAN>
					  <xsl:value-of select="Side"/>
				  </TRAN>

				  <TYPE>
					  <xsl:value-of select="Asset"/>
				  </TYPE>

				  <SECURITY>
					  <xsl:choose>
						  <xsl:when test="Asset = 'EquityOption'">
							  <xsl:value-of select="OSIOptionSymbol"/>
						  </xsl:when>
						  <xsl:when test="Asset = 'FixedIncome'">
							  <xsl:value-of select="''"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="Symbol"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </SECURITY>

				  <QUANTITY>
					  <xsl:value-of select="AllocatedQty"/>
				  </QUANTITY>

				  <PRICE>
					  <xsl:value-of select="AveragePrice"/>
				  </PRICE>

				  <TRADECURR>
					  <xsl:value-of select="CurrencySymbol"/>
				  </TRADECURR>

				  <CASHACC>
					  <xsl:value-of select="'JEFFERIES'"/>
				  </CASHACC>

				  <CUSIP>
					  <xsl:choose>
						  <xsl:when test="Asset = 'FixedIncome'">
							  <xsl:value-of select="CUSIP"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </CUSIP>

				  <ISIN>
					  <xsl:value-of select="ISIN"/>
				  </ISIN>

				  <BROKER>
					  <xsl:value-of select="CounterParty"/>
				  </BROKER>

				  <COMMISS>
					  <xsl:value-of select="CommissionCharged"/>
				  </COMMISS>

				  <SETTLECURR>
					  <xsl:choose>
						  <xsl:when test ="Asset = 'FXForward'">
							  <xsl:value-of select ="VsCurrencyName"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="CurrencySymbol"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </SETTLECURR>

				  <FXRATE>
					  <xsl:value-of select="ForexRate"/>
				  </FXRATE>

				  <NETSETTLEAMT>
					  <xsl:value-of select="NetAmount*ForexRate"/>
				  </NETSETTLEAMT>

				  <!-- system use only-->
				  <EntityID>
					  <xsl:value-of select="EntityID"/>
				  </EntityID>

			  </ThirdPartyFlatFileDetail>
		  </xsl:if>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
