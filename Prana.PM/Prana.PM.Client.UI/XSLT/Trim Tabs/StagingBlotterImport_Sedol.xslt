<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  
  <msxsl:script language="C#" implements-prefix="my">
    <msxsl:assembly name="System.Data"/>
    <msxsl:using namespace="System.Data"/>
    <msxsl:assembly name="System.Globalization"/>
    <msxsl:using namespace="System.Globalization"/>

    public  string NextBusinessDate(int year, int month, int date, int hours, int minuts, int seconds)
    {
    DateTime startTime = new DateTime(year, month, date, hours, minuts, seconds);
    string timeUtc = DateTime.UtcNow.ToString("HH");
    string timeEst = DateTime.Now.ToString("HH");

    if (Convert.ToInt16(timeUtc) - Convert.ToInt16(timeEst) == 4)
    {
    startTime = startTime.AddHours(4);
    }
    else
    {
    startTime = startTime.AddHours(5);
    }
    return startTime.ToString("yyyyMMdd-HH:mm:ss");
    }


  </msxsl:script>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name ="varSide">
          <xsl:value-of select ="translate(COL1,'&quot;','')"/>
        </xsl:variable>

        <xsl:if test="$varSide != 'Side' and translate(normalize-space(COL2),'&quot;','')!='USD' and translate(normalize-space(COL2),'&quot;','')!='CASH'">
          <PositionMaster>
            <OrderSideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Buy' or $varSide='buy' or $varSide='BUY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell' or $varSide='sell' or $varSide='SELL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell short' or $varSide='SELL SHORT'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varSide='Buy to Close' or $varSide='BUY TO CLOSE'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderSideTagValue>

            <xsl:variable name ="varSymbol">
              <xsl:value-of select ="translate(translate(normalize-space(COL2),'&quot;',''),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
            </xsl:variable>

			  <xsl:variable name ="varSSymbol">
				  <xsl:value-of select ="translate(normalize-space(COL2),'&quot;','')"/>
			  </xsl:variable>

			  <xsl:variable name ="varSEDOL">
				  <xsl:choose>
					  <xsl:when test="string-length($varSSymbol)='6'">
						  <xsl:value-of select="concat('0',$varSSymbol)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="translate(normalize-space(COL2),'&quot;','')"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </xsl:variable>

			  <Symbol>
				  <xsl:choose>
                <!--<xsl:when test="contains($varSymbol,' ')">-->
                <xsl:when test="$varSEDOL !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varSymbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            <SEDOLSymbol>
              <xsl:choose>
                <xsl:when test="$varSEDOL !=''">
                  <xsl:value-of select="$varSEDOL"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOLSymbol>
            <xsl:variable name ="varQuantity">
              <xsl:value-of select ="translate(COL3,'&quot;','')"/>
            </xsl:variable>

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varQuantity)">
                  <xsl:value-of select="number($varQuantity)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

        

            <xsl:variable name ="varOrderTypeTagValue">
              <xsl:value-of select ="translate(COL4,'&quot;','')"/>
            </xsl:variable>

            <OrderTypeTagValue>

              <xsl:choose>

                <xsl:when test ="$varOrderTypeTagValue='Market'">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Limit'">
                  <xsl:value-of select ="'2'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Stop'">
                  <xsl:value-of select ="'3'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Stop Limit'">
                  <xsl:value-of select ="'4'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Market on close'">
                  <xsl:value-of select ="'5'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='With or without'">
                  <xsl:value-of select ="'6'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Limit or better'">
                  <xsl:value-of select ="'7'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Limit with or without'">
                  <xsl:value-of select ="'8'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='On basis'">
                  <xsl:value-of select ="'9'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Pegged'">
                  <xsl:value-of select ="'P'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="'1'"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderTypeTagValue>



			  <Venue>
				  <xsl:choose>
					  <xsl:when test="COL8='Drops'">
						  <xsl:value-of select="'Drops'"/>
					  </xsl:when>
					  <xsl:when test="COL8='Algo'">
						  <xsl:value-of select="'Algo'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Venue>

			  <VenueID>
				  <xsl:choose>
					  <xsl:when test="COL8='Drops'">
						  <xsl:value-of select="1"/>
					  </xsl:when>
					  <xsl:when test="COL8='Algo'">
						  <xsl:value-of select="41"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </VenueID>

            <CounterPartyName>
				<xsl:choose>
					<xsl:when test="COL7='VCGO' and COL9='QAMM'">
						<xsl:value-of select="'VCGOQAMM'"/>
					</xsl:when>

					<xsl:when test="COL7='VCGO' and COL9='DMA'">
						<xsl:value-of select="'VCGODMA'"/>
					</xsl:when>

					<xsl:when test="COL7='VCGO' and COL9='VWAP'">
						<xsl:value-of select="'VCGOVWAP'"/>
					</xsl:when>

					<xsl:when test="COL7='VCGO' and COL9='SOR'">
						<xsl:value-of select="'VCGOSOR'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="COL7"/>
					</xsl:otherwise>
				</xsl:choose>
            </CounterPartyName>


			  <CounterPartyID>
				  <xsl:choose>

					  <xsl:when test="COL7='VCGO' and COL9='QAMM'">
						  <xsl:value-of select="50"/>
					  </xsl:when>

					  <xsl:when test="COL7='VCGO' and COL9='DMA'">
						  <xsl:value-of select="58"/>
					  </xsl:when>

					  <xsl:when test="COL7='VCGO' and COL9='VWAP'">
						  <xsl:value-of select="88"/>
					  </xsl:when>

					  <xsl:when test="COL7='VCGO' and COL9='SOR'">
						  <xsl:value-of select="89"/>
					  </xsl:when>

					  <xsl:when test="COL7='MISL'">
						  <xsl:value-of select="92"/>
					  </xsl:when>
					  <xsl:when test="COL7='MISL-F'">
						  <xsl:value-of select="94"/>
					  </xsl:when>
					  <xsl:when test="COL7='PS'">
						  <xsl:value-of select="63"/>
					  </xsl:when>
					  <xsl:when test="COL7='PS-F'">
						  <xsl:value-of select="67"/>
					  </xsl:when>
					  <xsl:when test="COL7='VCGO'">
						  <xsl:value-of select="17"/>
					  </xsl:when>
					  <xsl:when test="COL7='VCGOQAMM'">
						  <xsl:value-of select="50"/>
					  </xsl:when>
					  <xsl:when test="COL7='VCGODMA'">
						  <xsl:value-of select="58"/>
					  </xsl:when>
					  <xsl:when test="COL7='VCGOVWAP'">
						  <xsl:value-of select="88"/>
					  </xsl:when>
					  <xsl:when test="COL7='VCGOSOR'">
						  <xsl:value-of select="89"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CounterPartyID>

            <HandlingInstruction>
              <xsl:value-of select="3"/>
            </HandlingInstruction>




            <Level1ID>
              <xsl:choose>
                <xsl:when test ="COL6 = 'TTAC'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test ="COL6 = 'TTAI'">
                  <xsl:value-of select="3"/>
                </xsl:when>

<xsl:when test ="COL6 = 'PF-1'">
                  <xsl:value-of select="4"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'PF-2'">
                  <xsl:value-of select="5"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'PF-3'">
                  <xsl:value-of select="6"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'PF-4'">
                  <xsl:value-of select="7"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'PF-5'">
                  <xsl:value-of select="8"/>
                </xsl:when>
				
					<xsl:when test ="COL6 = 'SP-1'">
                  <xsl:value-of select="9"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'SP-2'">
                  <xsl:value-of select="10"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'SP-3'">
                  <xsl:value-of select="11"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'SP-4'">
                  <xsl:value-of select="12"/>
                </xsl:when>
				
				<xsl:when test ="COL6 = 'SP-5'">
                  <xsl:value-of select="13"/>
                </xsl:when>
				
				

                <xsl:otherwise>
                  <xsl:value-of select ="-2147483648"/>
                </xsl:otherwise>

              </xsl:choose>
            </Level1ID>


            <Level2ID>
              <xsl:value-of select ="0"/>
            </Level2ID>

            <TradingAccountID>
              <xsl:value-of select="11"/>
            </TradingAccountID>

            <ExecutionInstruction>
              <xsl:value-of select="'F'"/>
            </ExecutionInstruction>

      

            <CumQty>
              <xsl:value-of select="0"/>
            </CumQty>

         

            <PranaMsgType>
              <xsl:value-of select="3"/>
            </PranaMsgType>
            <UserID>             
              <xsl:value-of select="'17'"/>           
            </UserID>

			  <TradeAttribute2>
				  <xsl:choose>
					  <xsl:when test="COL9 != '*'">
						  <xsl:value-of select="COL9"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </TradeAttribute2>


            <xsl:variable name="varSDateCheck">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varDateCheck">
              <xsl:choose>
                <xsl:when test="$varSDateCheck !='*'">
                  <xsl:value-of select="my:NextBusinessDate(number(substring($varSDateCheck,1,4)),number(substring($varSDateCheck,5,2)),number(substring($varSDateCheck,7,2)),number(substring($varSDateCheck,10,2)),number(substring($varSDateCheck,13,2)),number(substring($varSDateCheck,16,2)))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varSDateCheck"/>
                </xsl:otherwise>
              </xsl:choose>
              
            </xsl:variable>

            <xsl:variable name="varStartDate">
              <xsl:choose>
                <xsl:when test="$varSDateCheck !='*'">
                  <xsl:value-of select="$varDateCheck"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varSDateCheck"/>
                </xsl:otherwise>
              </xsl:choose>              
            </xsl:variable>
            <TradeAttribute3>
              <xsl:choose>
                <xsl:when test="$varStartDate != '*'">
                  <xsl:value-of select="$varStartDate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TradeAttribute3>


            <xsl:variable name="varEDateTimeCheck">
              <xsl:value-of select="COL11"/>
            </xsl:variable>


            <xsl:variable name="varEndTimeCheck">
              <xsl:choose>
                <xsl:when test="$varEDateTimeCheck !='*'">
                  <xsl:value-of select="my:NextBusinessDate(number(substring($varEDateTimeCheck,1,4)),number(substring($varEDateTimeCheck,5,2)),
                            number(substring($varEDateTimeCheck,7,2)),number(substring($varEDateTimeCheck,10,2)),number(substring($varEDateTimeCheck,13,2)),number(substring($varEDateTimeCheck,16,2)))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varEDateTimeCheck"/>
                </xsl:otherwise>
              </xsl:choose>
              
            </xsl:variable>

            <xsl:variable name="varEndTime">
              <xsl:choose>
                <xsl:when test="$varEDateTimeCheck !='*'">
                  <xsl:value-of select="$varEndTimeCheck"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varEDateTimeCheck"/>
                </xsl:otherwise>
              </xsl:choose>             
            </xsl:variable>
            <TradeAttribute4>
              <xsl:choose>
                <xsl:when test="$varEndTime != '*'">
                  <xsl:value-of select="$varEndTime"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TradeAttribute4>

            <!--<Text>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Text>-->
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>