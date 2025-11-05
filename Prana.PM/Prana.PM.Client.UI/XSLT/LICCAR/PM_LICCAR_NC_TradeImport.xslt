<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


  <xsl:template name="MonthCode">
    <xsl:param name="Month" />
    <xsl:param name="PutOrCall" />
    <xsl:if test="$PutOrCall='CALL'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='PUT'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'X'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="COL9"/>
		</xsl:variable>
		<xsl:variable name="Date">
			<xsl:value-of select="normalize-space(substring-before(substring-after(substring-after(normalize-space(COL10),' '),' '),' '))"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring($Date,4,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring($Date,1,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring($Date,7,2)"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="normalize-space(substring-before(normalize-space(COL10),' '))"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(normalize-space(substring-after(substring-after(substring-after(normalize-space(COL10),'/'),'/'),' ')),'#.00')"/>
		</xsl:variable>
		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth"/>
				<xsl:with-param name="PutOrCall" select="$PutORCall"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL13)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
				<xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL12 = 'Option'">
                  <xsl:value-of select="'EquityOption'" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

					<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL10)" />
                  </xsl:call-template>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
			
			<PBSymbol>
				<xsl:value-of select="$PB_COMPANY_NAME"/>
			</PBSymbol>
						<xsl:variable name="PB_FUND_NAME" select="COL4"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						
						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>
				
					<SideTagValue>			
						<xsl:choose>
							<xsl:when test="$varAsset='EquityOption'">
								<xsl:choose>
								<xsl:when test="$varSide='Buy' and COL8 = 'Open'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy' and COL8 = 'Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'  and COL8 = 'Open'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'  and COL8 = 'Close'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
								<xsl:when test="$varSide='Buy' ">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell' ">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

             
						<xsl:variable name="varFirstMoneyAmount">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL15)"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varFirstMoneyAmount div $Position"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="CostBasisForOption">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varFirstMoneyAmount + 100 * $Position"/>
							</xsl:call-template>
						</xsl:variable>

			<CostBasis>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">

                  <xsl:choose>
                    <xsl:when test="$CostBasisForOption &gt; 0">
                      <xsl:value-of select="$CostBasisForOption"/>
                    </xsl:when>
                    <xsl:when test="$CostBasisForOption &lt; 0">
                      <xsl:value-of select="$CostBasisForOption * (-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>

                  <xsl:choose>
                    <xsl:when test="$CostBasis &gt; 0">
                      <xsl:value-of select="$CostBasis"/>
                    </xsl:when>
                    <xsl:when test="$CostBasis &lt; 0">
                      <xsl:value-of select="$CostBasis * (-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
		</CostBasis>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL16)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

            <xsl:variable name="varOcfFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL19)"/>
              </xsl:call-template>
            </xsl:variable>

            <OcfFee>
              <xsl:choose>
                <xsl:when test="$varOcfFee &gt; 0">
                  <xsl:value-of select="$varOcfFee"/>
                </xsl:when>
                <xsl:when test="$varOcfFee &lt; 0">
                  <xsl:value-of select="$varOcfFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OcfFee>
			
			<xsl:variable name="varOcfFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL18)"/>
              </xsl:call-template>
            </xsl:variable>

            <OccFee>
              <xsl:choose>
                <xsl:when test="$varOcfFee &gt; 0">
                  <xsl:value-of select="$varOcfFee"/>
                </xsl:when>
                <xsl:when test="$varOcfFee &lt; 0">
                  <xsl:value-of select="$varOcfFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OccFee>
			
			
            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL2"/>
            </xsl:variable>


            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>
			
			 <xsl:variable name="varSettlementDate">
              <xsl:value-of select="COL3" />
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
            </PositionSettlementDate>

			<PBAssetType>
			 <xsl:value-of select="$varAsset"/>
			</PBAssetType>
			
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


