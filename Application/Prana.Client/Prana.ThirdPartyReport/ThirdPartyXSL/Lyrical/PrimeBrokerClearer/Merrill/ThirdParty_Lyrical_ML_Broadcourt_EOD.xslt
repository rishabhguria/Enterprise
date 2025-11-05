<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <AccountNumber>
          <xsl:value-of select="'Account Number:'"/>
        </AccountNumber>

        <DVPMint>
          <xsl:value-of select="'DVP Mint #:'"/>
        </DVPMint>

        <TICKER>
          <xsl:value-of select="'TICKER'"/>
        </TICKER>

        <CODE>
          <xsl:value-of select="'CODE'"/>
        </CODE>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <GrossMoney>
          <xsl:value-of select="'Gross Money'"/>
        </GrossMoney>

        <CommissionRate>
          <xsl:value-of select="'Commission Rate'"/>
        </CommissionRate>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <SECFee>
          <xsl:value-of select="'SEC Fee'"/>
        </SECFee>

        <NetMoney>
          <xsl:value-of select="'Net Money'"/>
        </NetMoney>

        <SECURITYDESCRIPTION>
          <xsl:value-of select="'SECURITY DESCRIPTION'"/>
        </SECURITYDESCRIPTION>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <DTCSettlement>
          <xsl:value-of select="'DTC Settlement'"/>
        </DTCSettlement>
		
		<AccountName>
          <xsl:value-of select="'AccountName'"/>
        </AccountName>
		
		<Custodian>
          <xsl:value-of select="'Custodian'"/>
        </Custodian>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='BGCE']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'EOD'"/>
          </xsl:variable>

          <xsl:variable name="THIRDPARTY_ACCOUNT_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name="THIRDPARTY_ACCOUNT_NO">
            <xsl:value-of select="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@NirvanaAccountName=$THIRDPARTY_ACCOUNT_NAME]/@AccountNumber"/>
          </xsl:variable>


          <AccountNumber>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_ACCOUNT_NO!=''">
                <xsl:value-of select="$THIRDPARTY_ACCOUNT_NO"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="AccountName"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNumber>


          <xsl:variable name="THIRDPARTY_DVP_NO">
            <xsl:value-of select="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@NirvanaAccountName=$THIRDPARTY_ACCOUNT_NAME]/@DVPMintMLBroad"/>
          </xsl:variable>
          <DVPMint>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_DVP_NO!=''">
                <xsl:value-of select="concat(concat('=&quot;',$THIRDPARTY_DVP_NO),'&quot;')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="AccountName"/>
              </xsl:otherwise>
            </xsl:choose>
          </DVPMint>

          <TICKER>
            <xsl:value-of select="Symbol"/>
          </TICKER>

          <CODE>
            <xsl:value-of select="translate(Side,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
          </CODE>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <xsl:variable name="varPrice">
            <xsl:value-of select="format-number(AveragePrice,'#.####')"/>
          </xsl:variable>
          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <GrossMoney>
            <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select ="format-number((AllocatedQty * $varPrice),'#.##')"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select ="format-number((AllocatedQty * $varPrice),'#.##')"/>
              </xsl:when>
            </xsl:choose>
          </GrossMoney>

          <xsl:variable name="varCommissionRate">
<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) div AllocatedQty"/>
</xsl:variable>
          <CommissionRate>
            <xsl:value-of select="$varCommissionRate"/>
          </CommissionRate>

          <xsl:variable name="varCommission">
            <xsl:value-of select="format-number(CommissionCharged,'#.##')"/>
          </xsl:variable>

			<xsl:variable name="varNewCommission">
				<xsl:choose>
					<xsl:when test="$varCommission=''">
						<xsl:value-of select="0"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varCommission"/>
					</xsl:otherwise>

				</xsl:choose>
			</xsl:variable>
          <Commission>
            <xsl:value-of select="$varNewCommission"/>
          </Commission>

          <xsl:variable name="varSECFee">
            <xsl:choose>
              <xsl:when test="number(StampDuty)">
                <xsl:value-of select="format-number(StampDuty,'#.##')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
	  <xsl:variable name="newvarSecFee">
	   <xsl:choose>
	  <xsl:when test="$varSECFee=''">
	  <xsl:value-of select="0"/>																																																																																											
	  </xsl:when>
	  <xsl:otherwise>
	  <xsl:value-of select="$varSECFee"/>
	  </xsl:otherwise>
	  
	  </xsl:choose>
	  </xsl:variable>
	  
          <SECFee>
             <xsl:value-of select="$newvarSecFee"/>
          </SECFee>

          <NetMoney>
            <xsl:choose>
              <xsl:when test ="Side='Buy'">				
					<xsl:value-of select ="format-number(((AllocatedQty * $varPrice) + $varNewCommission + $newvarSecFee),'#.##')"/>				
              </xsl:when>
              <xsl:when test ="Side='Sell'">
					<xsl:value-of select ="format-number(((AllocatedQty * $varPrice) - ($varNewCommission + $newvarSecFee)),'#.##')"/>			
              </xsl:when>
            </xsl:choose>
          </NetMoney>

          <SECURITYDESCRIPTION>
            <xsl:value-of select="FullSecurityName"/>
          </SECURITYDESCRIPTION>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <DTCSettlement>
            <xsl:value-of select="'DTC 161'"/>
          </DTCSettlement>
		  
		  <AccountName>
           <xsl:value-of select="$THIRDPARTY_ACCOUNT_NAME"/>
          </AccountName>
		
			<xsl:variable name="THIRDPARTY_CUSTODIAN">
				<xsl:value-of select="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@NirvanaAccountName=$THIRDPARTY_ACCOUNT_NAME]/@Custodian"/>
			</xsl:variable>
		
			<Custodian>
			  <xsl:choose>
                <xsl:when test="$THIRDPARTY_CUSTODIAN!=''">
                  <xsl:value-of select="$THIRDPARTY_CUSTODIAN"/>
                </xsl:when>
              
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
			</Custodian>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>