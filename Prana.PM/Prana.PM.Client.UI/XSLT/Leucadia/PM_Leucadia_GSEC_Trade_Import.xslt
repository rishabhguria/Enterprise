<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
		  <xsl:if test ="COL1 != 'Account Number'">


			  <PositionMaster>
				  <!--   Fund -->
				  <!--fundname section-->
				  <xsl:variable name = "PB_FUND_NAME" >
					  <xsl:value-of select="COL1"/>
				  </xsl:variable>
				  
				  
				  <xsl:variable name="PRANA_FUND_NAME">
					  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSec']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				  </xsl:variable>

				  <xsl:variable name="PRANA_STRATEGY_NAME">
					  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name='GSec']/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
				  </xsl:variable>
				  
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME=''">
						  <FundName>
							  <xsl:value-of select='$PB_FUND_NAME'/>
						  </FundName>
					  </xsl:when>
					  <xsl:otherwise>
						  <FundName>
							  <xsl:value-of select='$PRANA_FUND_NAME'/>
						  </FundName>
					  </xsl:otherwise>
				  </xsl:choose>

				  <!--<xsl:choose>
            <xsl:when test ="COL3 = 'STK'">
              <Symbol>
                <xsl:value-of select="COL1"/>
              </Symbol>
            </xsl:when>
            <xsl:when test ="COL3 = 'SO'">
              <xsl:variable name = "varLength" >
                <xsl:value-of select="string-length(COL1)"/>
              </xsl:variable>
              <xsl:variable name = "varAfter" >
                <xsl:value-of select="substring(COL1,($varLength)-1,2)"/>
              </xsl:variable>
              <xsl:variable name = "varBefore" >
                <xsl:value-of select="substring(COL1,1,($varLength)-2)"/>
              </xsl:variable>
              <Symbol>
                <xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
              </Symbol>
            </xsl:when>
            <xsl:when test ="COL3 = 'FUT'">
              <xsl:variable name ="varFut">
                <xsl:value-of select ="translate(COL1,'/','')"/>
              </xsl:variable>
              <xsl:variable name ="varFutLen">
                <xsl:value-of select ="string-length($varFut)"/>
              </xsl:variable>
              <xsl:variable name ="varFutSymbol">
                <xsl:value-of select ="concat(substring($varFut,1,2),' ',substring($varFut,3,1),substring($varFut,$varFutLen,1))"/>
              </xsl:variable>
              <Symbol>
                <xsl:value-of select="$varFutSymbol"/>
              </Symbol>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="COL1"/>
              </Symbol>
            </xsl:otherwise>
          </xsl:choose >-->

				  <!--<xsl:choose>
					  <xsl:when test ="COL4 = 'Date' or COL4 = '*'">
						  <PositionStartDate>
							  <xsl:value-of select="''"/>
						  </PositionStartDate>
					  </xsl:when>
					  <xsl:otherwise>
						  <PositionStartDate>
							  <xsl:value-of select="COL4"/>
						  </PositionStartDate>
					  </xsl:otherwise>
				  </xsl:choose>-->
				  
				  <xsl:variable name="Month">
					  <xsl:call-template name="MonthName">
						  <xsl:with-param name="Month" select="substring-before(COL4,' ')"/>
					  </xsl:call-template>
				  </xsl:variable>

				  <PositionStartDate>
					  <xsl:value-of select="concat($Month,'/',substring-before(substring-after(COL4,' '),','),'/',substring-after(substring-after(COL4,' '),' '))"/>
				  </PositionStartDate>

				  <xsl:variable name="PB_Symbol" select="COL30"/>
				  <xsl:variable name="PRANA_SYMBOL_NAME">
					  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSec']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
				  </xsl:variable>

				 
				  <xsl:choose>
					  <xsl:when test ="COL16 = 'FUTURE'">
						  <Symbol>
							  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="''"/>
						  </IDCOOptionSymbol>
					  </xsl:when>
					  <xsl:when test ="COL16 = 'EQUITY' ">
						  <Symbol>
							  <xsl:choose>
								  <xsl:when test ="$PRANA_SYMBOL_NAME = ''">
									  <xsl:value-of select="COL3"/>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="''"/>
						  </IDCOOptionSymbol>
					  </xsl:when>
					  <xsl:when test ="COL16 = 'OPTION' ">
						  <Symbol>
							  <xsl:value-of select="''"/>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="concat(substring(COL17,1,21),'U')"/>
						  </IDCOOptionSymbol>
					  </xsl:when>
					  <xsl:otherwise>
						  <Symbol>
							  <xsl:value-of select="COL3"/>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="''"/>
						  </IDCOOptionSymbol>
					  </xsl:otherwise>
				  </xsl:choose>

				  <Strategy>
					  <xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
				  </Strategy>

				  <PBSymbol>
					  <xsl:value-of select="COL3"/>
				  </PBSymbol>

				  <!--QUANTITY-->

				  <xsl:choose>
					  <xsl:when test="COL5 &lt; 0">
						  <NetPosition>
							  <xsl:value-of select="COL5 * (-1)"/>
						  </NetPosition>
					  </xsl:when>
					  <xsl:when test="COL5 &gt; 0">
						  <NetPosition>
							  <xsl:value-of select="COL5"/>
						  </NetPosition>
					  </xsl:when>
					  <xsl:otherwise>
						  <NetPosition>
							  <xsl:value-of select="0"/>
						  </NetPosition>
					  </xsl:otherwise>
				  </xsl:choose>

				  <!--Side-->
				  <xsl:choose>

					 
					  <xsl:when test="(COL2='BUY TO CLOSE' or COL2='COVER BUY') and  COL16='TO CLOSE'">
						  <SideTagValue>
							  <xsl:value-of select="'B'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="(COL2='BUY TO OPEN' or COL2='BUY') and COL16='TO OPEN'">
						  <SideTagValue>
							  <xsl:value-of select="'A'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="(COL2='SELL TO CLOSE') and COL16='TO CLOSE'">
						  <SideTagValue>
							  <xsl:value-of select="'D'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="(COL2='SELL TO OPEN' or COL2='SELL')  and COL18='TO OPEN'">
						  <SideTagValue>
							  <xsl:value-of select="'C'"/>
						  </SideTagValue>
					  </xsl:when>

					  <xsl:when test="COL2='BUY'">
						  <SideTagValue>
							  <xsl:value-of select="'1'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="COL2='SELL'">
						  <SideTagValue>
							  <xsl:value-of select="'2'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="COL2='SHORT SELL'">
						  <SideTagValue>
							  <xsl:value-of select="'5'"/>
						  </SideTagValue>
					  </xsl:when>


					  <xsl:when test="(COL2='BUY TO CLOSE' or COL2='COVER BUY')">
						  <SideTagValue>
							  <xsl:value-of select="'B'"/>
						  </SideTagValue>
					  </xsl:when>
					  
					  <xsl:otherwise>
						  <SideTagValue>
							  <xsl:value-of select="''"/>
						  </SideTagValue>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when test ="boolean(number(COL6))">
						  <CostBasis>
							  <xsl:value-of select="COL6"/>
						  </CostBasis>
					  </xsl:when>
					  <xsl:otherwise>
						  <CostBasis>
							  <xsl:value-of select="0"/>
						  </CostBasis>
					  </xsl:otherwise>
				  </xsl:choose>

				  <!--<xsl:choose>
            <xsl:when test ="number(COL5) and number(COL5) != 0 and number(COL9)">
              <CostBasis>
                <xsl:value-of select="(COL9 div COL5) * -1"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>-->

				  <xsl:choose>
					  <xsl:when test="COL7&gt; 0">
						  <Commission>
							  <xsl:value-of select="COL7"/>
						  </Commission>
					  </xsl:when>
					  <xsl:when test="COL7 &lt; 0">
						  <Commission>
							  <xsl:value-of select="COL7*(-1)"/>
						  </Commission>
					  </xsl:when>
					  <xsl:otherwise>
						  <Commission>
							  <xsl:value-of select="0"/>
						  </Commission>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:variable name ="varMiscSum">
					  <xsl:value-of select ="COL8 + COL9 + COL10 "/>
				  </xsl:variable>

				  <xsl:choose>
					  <xsl:when test="$varMiscSum &lt; 0">
						  <MiscFees>
							  <xsl:value-of select="$varMiscSum*(-1)"/>
						  </MiscFees>
					  </xsl:when>
					  <xsl:when test="$varMiscSum &gt; 0">
						  <MiscFees>
							  <xsl:value-of select="$varMiscSum"/>
						  </MiscFees>
					  </xsl:when>
					  <xsl:otherwise>
						  <MiscFees>
							  <xsl:value-of select="0"/>
						  </MiscFees>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when test="COL11 &gt; 0">
						  <StampDuty>
							  <xsl:value-of select="COL11 *(-1)"/>
						  </StampDuty>
					  </xsl:when>
					  <xsl:when test="COL11 &lt; 0">
						  <StampDuty>
							  <xsl:value-of select="COL11"/>
						  </StampDuty>
					  </xsl:when>
					  <xsl:otherwise>
						  <StampDuty>
							  <xsl:value-of select="0"/>
						  </StampDuty>
					  </xsl:otherwise>
				  </xsl:choose>


				  <CounterPartyID>
					  <xsl:value-of select="'16'"/>
				  </CounterPartyID>




			  </PositionMaster>
		  </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
