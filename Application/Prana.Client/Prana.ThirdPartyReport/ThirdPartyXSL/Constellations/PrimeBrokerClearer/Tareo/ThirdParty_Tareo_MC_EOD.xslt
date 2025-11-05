<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <RecordType>
          <xsl:value-of select ="'ACCOUNT'"/>
        </RecordType>

        <Bank>
          <xsl:value-of select ="'MORGANSTANLEY'"/>
        </Bank>

        <Branch>
          <xsl:value-of select ="'MOUNTAINCOVE'"/>
        </Branch>

        <Customer>
          <xsl:value-of select ="'MOUNTAINCOVE'"/>
        </Customer>

        <Deposit>
          <xsl:value-of select ="'TRADING'"/>
        </Deposit>

        <AccountType>
          <xsl:value-of select ="''"/>
        </AccountType>
		
		<CURRENCY>
          <xsl:value-of select ="'USD'"/>
        </CURRENCY>

        <SecuritySymbol>
          <xsl:value-of select ="'USD'"/>
        </SecuritySymbol>

        <SecurityType>
          <xsl:value-of select ="'0'"/>
        </SecurityType>

        <Price>
          <xsl:value-of select ="'0'"/>
        </Price>

        <Position>
          <xsl:value-of select ="'0'"/>
        </Position>

        <CostBasis>
          <xsl:value-of select ="'0'"/>
        </CostBasis>

        <OptionRoot>
          <xsl:value-of select ="'0'"/>
        </OptionRoot>

        <StrikePrice>
          <xsl:value-of select ="'0'"/>
        </StrikePrice>

        <PutCall>
          <xsl:value-of select ="'0'"/>
        </PutCall>

        <ExpirationDate>
          <xsl:value-of select ="''"/>
        </ExpirationDate>

        <Exchange>
          <xsl:value-of select ="'E'"/>
        </Exchange>

        <ExternalSymbol1>
          <xsl:value-of select ="'C'"/>
        </ExternalSymbol1>

        <ExternalSymbolType1>
          <xsl:value-of select ="''"/>
        </ExternalSymbolType1>
		
        <ExternalSymbolType2>
          <xsl:value-of select ="''"/>
        </ExternalSymbolType2>
		
		<ExternalSymbolType3>
          <xsl:value-of select ="''"/>
        </ExternalSymbolType3>
		
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

          <RecordType>
            <xsl:value-of select ="'POSITION'"/>
          </RecordType>

          <Bank>
            <xsl:value-of select ="'MORGANSTANLEY'"/>
          </Bank>

          <Branch>
            <xsl:value-of select ="'MOUNTAINCOVE'"/>
          </Branch>

          <Customer>
            <xsl:value-of select ="'MOUNTAINCOVE'"/>
          </Customer>

          <Deposit>
            <xsl:value-of select ="'TRADING'"/>
          </Deposit>

          <AccountType>
            <xsl:choose>
              <xsl:when test="LongShort='Long'">
                <xsl:value-of select="'2'"/>
              </xsl:when>

              <xsl:when test="LongShort='Short'">
                <xsl:value-of select="'3'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountType>
		  
		  <CURRENCY>
            <xsl:value-of select ="'USD'"/>
          </CURRENCY>

          <SecuritySymbol>
            <xsl:value-of select="substring-before(translate(BloombergSymbol,$varSmall,$varCapital),' EQUITY')"/>
          </SecuritySymbol>

          <SecurityType>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'1'"/>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'2'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityType>

          <Price>
            <xsl:value-of select ="format-number(MarkPrice,'#.####')"/>
          </Price>

          <Position>
            <xsl:value-of select ="OpenPosition"/>
          </Position>

            <CostBasis>
            <xsl:choose>
              <xsl:when test="number(TotalCost_Local)">
                <xsl:value-of select ="format-number(TotalCost_Local,'#.0000')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </CostBasis>

          <OptionRoot>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="UnderLyingSymbol"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OptionRoot>

          <StrikePrice>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </StrikePrice>

          <PutCall>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
				  <xsl:choose>
					<xsl:when test="PutOrCall='PUT'">
						<xsl:value-of select="'P'"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="'C'"/>
					</xsl:otherwise>
				  </xsl:choose>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PutCall>

          <ExpirationDate>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExpirationDate>

          <Exchange>
            <xsl:value-of select ="''"/>
          </Exchange>

          <ExternalSymbol1>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="SEDOLSymbol"/>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSISymbol"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExternalSymbol1>

          <ExternalSymbolType1>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'S'"/>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExternalSymbolType1>

  <ExternalSymbolType2>
          <xsl:value-of select ="''"/>
        </ExternalSymbolType2>
		
		<ExternalSymbolType3>
          <xsl:value-of select ="''"/>
        </ExternalSymbolType3>
		
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
