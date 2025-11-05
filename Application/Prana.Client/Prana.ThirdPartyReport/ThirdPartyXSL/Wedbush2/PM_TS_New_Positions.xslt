<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
		  <xsl:if test ="COL1 != 'Account Number'">


			  <PositionMaster>
				  <!--   Fund -->
				  <FundName>
					  <xsl:value-of select="''"/>
				  </FundName>


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

				  <xsl:choose>
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
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when test ="COL7 = 'EQUITY' ">
						  <Symbol>
							  <xsl:value-of select="COL3"/>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="''"/>
						  </IDCOOptionSymbol>
					  </xsl:when>
					  <xsl:when test ="COL7 = 'OPTION' ">
						  <Symbol>
							  <xsl:value-of select="''"/>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="concat(COL8,'U')"/>
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




				  <PBSymbol>
					  <xsl:value-of select="concat(COL3,'Option Symbol--',COL8) "/>
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
					  <xsl:when test="COL2='BUY TO CLOSE'">
						  <SideTagValue>
							  <xsl:value-of select="'B'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="COL2='BUY  TO OPEN'">
						  <SideTagValue>
							  <xsl:value-of select="'A'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="COL2='SELL TO CLOSE'">
						  <SideTagValue>
							  <xsl:value-of select="'D'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="COL2='SELL TO OPEN'">
						  <SideTagValue>
							  <xsl:value-of select="'C'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:otherwise>
						  <SideTagValue>
							  <xsl:value-of select="'sell'"/>
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

				  <!--<xsl:choose>
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
				  </xsl:choose>-->


				  <CounterPartyID>
					  <xsl:value-of select="'19'"/>
				  </CounterPartyID>




			  </PositionMaster>
		  </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
