<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
	  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
        <ThirdPartyFlatFileDetail>
         
          <RowHeader>
            <xsl:value-of select="'true'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


	      	<AccountName>
	            <xsl:value-of select="AccountName"/>
          </AccountName>
		  
          <Shortname>
            <xsl:value-of select="SecurityDescription"/>
          </Shortname>


          <Symbol>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>

          <SEDOL>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select ="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="SEDOLSymbol"/>
              </xsl:otherwise>
            </xsl:choose>

          </SEDOL>
          <BloombergID>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select ="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="BloombergSymbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </BloombergID>
          <RICID>
            <xsl:value-of select="''"/>
          </RICID>
          <Side>
            <xsl:value-of select="PositionIndicator"/>
          </Side>
          <Qty>
            <xsl:value-of select="OpenPositions"/>
          </Qty>
          <EntryPrice>
            <xsl:value-of select="AvgPrice"/>
          </EntryPrice>
          <Curr>
            <xsl:value-of select="LocalCurrency"/>
          </Curr>
          <PosType>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select ="'OPT'"/>
              </xsl:when>
              <xsl:when test="AssetClass='Equity'">
                <xsl:value-of select ="'EQT'"/>
              </xsl:when>			  
			  <xsl:when test="AssetClass='FX'">
                <xsl:value-of select ="'FX'"/>
              </xsl:when>	

             <xsl:when test="AssetClass='PrivateEquity'">
                <xsl:value-of select ="'WARRANT/PRIVATE EQUITY'"/>
              </xsl:when>	
			  
			  <xsl:when test="AssetClass='FXForward'">
                <xsl:value-of select ="'FX FORWARD'"/>
              </xsl:when>	
             <xsl:when test="AssetClass='EquitySwap'">
                <xsl:value-of select ="'EQUITY SWAP'"/>
              </xsl:when>				  
			  
              <xsl:otherwise>
                <xsl:value-of select ="translate(AssetClass,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
              </xsl:otherwise>
            </xsl:choose>
          </PosType>
		  

         <Exch>		 
						<xsl:value-of select="Exchange"/>
          </Exch>

          <Underlier>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select ="UnderLyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Underlier>

          <CP>
            <xsl:value-of select="substring(PutOrCall,1,1)"/>
          </CP>

          <xsl:variable name="TradeDate" select="substring-before(ExpirationDate,'T')"/>
          <ExpDate>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select="concat(substring-before(substring-after($TradeDate,'-'),'-'),'-',substring-after(substring-after($TradeDate,'-'),'-'),'-',substring-before($TradeDate,'-'))"/>
              </xsl:when>
			  <xsl:when test="AssetClass='FXForward'">
                <xsl:value-of select="concat(substring-before(substring-after($TradeDate,'-'),'-'),'-',substring-after(substring-after($TradeDate,'-'),'-'),'-',substring-before($TradeDate,'-'))"/>
              </xsl:when>
              <xsl:otherwise>
                 <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExpDate>

          <StrikePrice>
            <xsl:choose>
              <xsl:when test="AssetClass='EquityOption'">
                <xsl:value-of select ="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </StrikePrice>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>
		

      </xsl:for-each>
 
    </ThirdPartyFlatFileDetailCollection>
	
  </xsl:template>
</xsl:stylesheet>