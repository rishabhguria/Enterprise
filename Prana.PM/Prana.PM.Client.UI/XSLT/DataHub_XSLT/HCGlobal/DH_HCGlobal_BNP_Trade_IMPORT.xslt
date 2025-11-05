<?xml version="1.0" encoding="utf-8" ?>
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
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<!--APTO1 01/19/24 C2.5-->
	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL11),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL11),' '),'/'),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(substring-after(normalize-space(COL11),' '),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space(COL11),' '),'/'),'/'),' ')"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(normalize-space(COL11),' '),'/'),'/'),' '),1,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(substring-after(normalize-space(COL11),' '),'/'),'/'),' '),2),'#.00')"/>
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


      <xsl:for-each select="//PositionMaster">
		  
        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
		
			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="COL9"/>
			  </xsl:variable>

			  <xsl:variable name="varSymbol">
				  <xsl:value-of select="COL6"/>
			  </xsl:variable>
			
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>
			  

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>
					 
					  <xsl:when test="$varSymbol!=''">
						  <xsl:value-of select="$varSymbol"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <xsl:variable name="varSide">
				  <xsl:value-of select="COL4"/>
			  </xsl:variable>

			  <SideTagValue>
				  <xsl:choose>
					  <xsl:when test="contains($varSide,'Buy')">
						  <xsl:value-of select="'1'"/>
					  </xsl:when>
					  <xsl:when test="contains($varSide,'Sell')">
						  <xsl:value-of select="'2'"/>
					  </xsl:when>					
				  </xsl:choose>
            </SideTagValue>

            <!--<xsl:variable name="PB_FUND_NAME" select="COL2"/>-->

			  <xsl:variable name="PB_FUND_NAME" select="'The Purpleville Foundation: 14462208'"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
			

			<PositionStartDate>
				<xsl:value-of select="COL2"/>
			</PositionStartDate>


            <PositionSettlementDate>
              <xsl:value-of select="COL3"/>
            </PositionSettlementDate>
			
            <NetPosition>
              <xsl:choose>
                <xsl:when  test="number($varQuantity)">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="number($AvgPrice)">
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

			  <xsl:variable name="NetNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="normalize-space(COL14)"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <NetAmount>
				  <xsl:choose>
					  <xsl:when test="number($NetNotionalValue)">
						  <xsl:value-of select="$NetNotionalValue"/>
					  </xsl:when>
					
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetAmount>

			  <xsl:variable name="NetNotionalValueBase">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="''"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <NetAmountBase>
				  <xsl:choose>
					  <xsl:when test="number($NetNotionalValueBase)">
						  <xsl:value-of select="$NetNotionalValueBase"/>
					  </xsl:when>
					 
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetAmountBase>
			  <xsl:variable name="commission">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL12"/>
                </xsl:call-template>
              </xsl:variable>
			  
              <Commission>
                 <xsl:choose>
                <xsl:when  test="number($commission)">
                  <xsl:value-of select="$commission"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
              </Commission>
			  
            <!--<CurrencyName>
              <xsl:value-of select="COL38"/>
            </CurrencyName>-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


