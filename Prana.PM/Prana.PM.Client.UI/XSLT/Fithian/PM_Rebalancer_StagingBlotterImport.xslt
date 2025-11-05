<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  
 <msxsl:script language="C#" implements-prefix="my">
		public int RoundOff(double Qty)
		{
		return (int)Math.Floor(Qty);
		}
	</msxsl:script>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name ="varSide">
          <xsl:value-of select ="translate(COL4,'&quot;','')"/>
        </xsl:variable>

        <xsl:variable name ="varQty">
          <xsl:value-of select ="translate(COL5,'&quot;','')"/>
        </xsl:variable>

        <xsl:if test="number($varQty)">
          <PositionMaster>
            <OrderSideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varSide='BuyToClose'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderSideTagValue>

            <Symbol>
              <xsl:value-of select="translate(normalize-space(COL3),'&quot;','')"/>
            </Symbol>

            <xsl:variable name ="varRoundQty">				
				  <xsl:value-of select ="my:RoundOff($varQty)"/>
			  </xsl:variable>

			  <Quantity>
              <xsl:choose>
                <xsl:when test="number($varRoundQty)">
                  <xsl:value-of select="number($varRoundQty)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <OrderTypeTagValue>
              <xsl:value-of select ="'1'"/>
            </OrderTypeTagValue>

            <xsl:variable name ="varPrice">
              <xsl:value-of select ="translate(COL6,'&quot;','')"/>
            </xsl:variable>

            <Price>
              <xsl:value-of select ="$varPrice"/>
            </Price>

            <Venue>
              <xsl:value-of select="'Drops'"/>
            </Venue>

            <VenueID>
              <xsl:value-of select="1"/>
            </VenueID>

            <xsl:variable name ="varAccountID">
              <xsl:value-of select ="translate(COL1,'&quot;','')"/>
            </xsl:variable>

            <Level1ID>
              <xsl:value-of select ="$varAccountID"/>
            </Level1ID>

            <Level2ID>
              <xsl:value-of select ="0"/>
            </Level2ID>



            <CounterPartyName>
              <xsl:choose>
                <xsl:when test="$varAccountID='30' or $varAccountID='31' or $varAccountID='32'">
                  <xsl:value-of select="'JONE'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="'CHAS'"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyName>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="$varAccountID='30' or $varAccountID='31' or $varAccountID='32'">
                  <xsl:value-of select="98"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="112"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>



            <HandlingInstruction>
              <xsl:value-of select="'1'"/>
            </HandlingInstruction>



            <TradingAccountID>
              <xsl:value-of select="11"/>
            </TradingAccountID>

            <ExecutionInstruction>
              <xsl:value-of select="'5'"/>
            </ExecutionInstruction>

            <FXRate>
							<xsl:value-of select="COL9"/>
						</FXRate>
						
						<FXConversionMethodOperator>
							<xsl:value-of select="COL10"/>
						</FXConversionMethodOperator>


            <CumQty>
              <xsl:value-of select="0"/>
            </CumQty>

            <PranaMsgType>
              <xsl:value-of select="3"/>
            </PranaMsgType>

            <UserID>
              <xsl:value-of select="'17'"/>
            </UserID>



            <!--<Text>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Text>-->
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>