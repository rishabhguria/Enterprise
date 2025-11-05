<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <!--<xsl:template match="/ThirdPartyFlatFileDetailCollection">-->
	   <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
			
			<xsl:variable name="PB_NAME">
				<xsl:value-of select="'NAV'"/>
			</xsl:variable>

			<xsl:variable name="varAccountName" select="AccountName"/>

			
			<xsl:variable name="PRANA_FUND_NAME">
				<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$varAccountName]/@PBFundName" />
			</xsl:variable>
			<Holder>
				<xsl:choose>
					<xsl:when test="$PRANA_FUND_NAME!=''">
						<xsl:value-of select="$PRANA_FUND_NAME" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varAccountName" />
					</xsl:otherwise>
				</xsl:choose>
			</Holder>
			
			<xsl:variable name="PRANA_ACCT">
				<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$varAccountName]/@PBFundCode" />
			</xsl:variable>
			<ACCT>
				<xsl:choose>
					<xsl:when test="$PRANA_ACCT!=''">
						<xsl:value-of select="$PRANA_ACCT" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''" />
					</xsl:otherwise>
				</xsl:choose>
			</ACCT>
			
			<NAMEREQURIED>
				<xsl:value-of select="''" />			
			</NAMEREQURIED>
			
			<RTE>
				<xsl:value-of select="''" />			
			</RTE>
				
			<STA>
				<xsl:value-of select="''" />			
			</STA>
				
			<CLUP>
				<xsl:value-of select="''" />			
			</CLUP>
			
			<BS>
				<xsl:choose>
					<xsl:when test="Side='Buy'">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:when test="Side='Sell'">
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:when test="Side='Buy to Close'">
						<xsl:value-of select="'BCO'"/>
					</xsl:when>

					<xsl:when test="Side='Sell short'">
						<xsl:value-of select="'SS'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</BS>
			
			<SL>
				<xsl:value-of select="''" />			
			</SL>
				
			<FLRINST>
				<xsl:value-of select="''" />			
			</FLRINST>			
			
			<QTY>
				<xsl:value-of select="Quantity"/>
			</QTY>
			
			<SYMBOL>
				<xsl:value-of select="Symbol"/>
			</SYMBOL>
			
			<EXP>
				<xsl:value-of select="''"/>
			</EXP>
			
			<STRK>
				<xsl:value-of select="''"/>
			</STRK>
			
			<OC>
				<xsl:value-of select="''"/>
			</OC>
			
			<PRICE>
				<xsl:value-of select="Price"/>
			</PRICE>
			
			<SOL>
				<xsl:value-of select="''"/>
			</SOL>
		
			
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

